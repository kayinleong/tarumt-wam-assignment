using Microsoft.AspNetCore.Mvc;
using Tarumt.WAM.Assignment.Infrastructure.Models;
using Tarumt.WAM.Assignment.Infrastructure.Requests;
using Tarumt.WAM.Assignment.Infrastructure.Services;
using Microsoft.Extensions.Options;
using Tarumt.WAM.Assignment.Infrastructure.Constants;
using Microsoft.AspNetCore.Authorization;

namespace Tarumt.WAM.Assignment.Controllers
{
    [Authorize]
    public class TicketController(MovieShowtimeService movieShowtimeService, TicketService ticketService, IOptions<StripeSettings> stripeSettings) : Controller
    {
        [HttpGet("/ticket/")]
        public ActionResult Index(int pageNumber = 1, int pageSize = 3, string keyword = "")
        {
            var user = HttpContext.Items["User"] as User;
            var tickets = ticketService.GetAllByStatusAndUserAsync(pageNumber, pageSize, keyword, user!);
            return View(tickets);
        }

        [HttpGet("/ticket/confirm_order/")]
        public async Task<ActionResult> ConfirmOrder([FromQuery] MovieShowtimeAddToCartRequest movieShowtimeAddToCartRequest)
        {
            var user = HttpContext.Items["User"] as User;

            try
            {
                var movieShowtime = await movieShowtimeService.GetByIdAsync(movieShowtimeAddToCartRequest.MovieShowtimeId);
                var ticket = new Ticket()
                {
                    SeatNumbers = movieShowtimeAddToCartRequest.SelectedSeat,
                    MovieShowtime = movieShowtime,
                    User = user!,
                    Status = TicketEnum.PENDING,
                };

                await ticketService.CreateAsync(ticket);
                movieShowtimeAddToCartRequest.TicketId = ticket.Id;

                ViewBag.MovieShowtime = await movieShowtimeService.GetByIdAsync(movieShowtimeAddToCartRequest.MovieShowtimeId);
                ViewBag.PublishableKey = stripeSettings.Value.PublishableKey;
                return View(movieShowtimeAddToCartRequest);
            }
            catch
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        [HttpGet("/ticket/success/")]
        public async Task<IActionResult> Success([FromQuery] string id)
        {
            var user = HttpContext.Items["User"] as User;

            var ticket = await ticketService.GetByIdAsync(id);
            if (ticket.Status == TicketEnum.PAID)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            var movieShowtime = await movieShowtimeService.GetByIdAsync(ticket.MovieShowtime.Id);

            try
            {
                movieShowtime.BookAvailableSeats(ticket.SeatNumbers);
                await movieShowtimeService.UpdateByIdAsync(movieShowtime);

                ticket.Status = TicketEnum.PAID;
                await ticketService.UpdateByIdAsync(ticket);
            }
            catch
            {
                return RedirectToAction(nameof(Cancel), "Ticket");
            }

            ViewBag.MovieShowtime = movieShowtime;
            return View(ticket);
        }

        [HttpGet("/ticket/cancel/")]
        public IActionResult Cancel()
        {
            return View();
        }
    }
}
