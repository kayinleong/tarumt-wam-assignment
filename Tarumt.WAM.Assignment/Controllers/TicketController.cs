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
    public class TicketController(UserLogService userLogService, MovieShowtimeService movieShowtimeService, TicketService ticketService, IOptions<StripeSettings> stripeSettings) : Controller
    {
        [HttpGet("/ticket/")]
        public async Task<ActionResult> Index(int pageNumber = 1, int pageSize = 3, string keyword = "")
        {
            var user = HttpContext.Items["User"] as User;
            var tickets = ticketService.GetAllByStatusAndUserAsync(pageNumber, pageSize, keyword, user!);

            await userLogService.CreateAsync(new()
            {
                Message = $"User POST TicketController.Index(PageNumber={pageNumber},PageSize={pageSize},Keyword={keyword})",
                Type = UserLogEnum.NORMAL,
                User = user!,
            });

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

                await userLogService.CreateAsync(new()
                {
                    Message = $"User POST TicketController.ConfirmOrder(...)",
                    Type = UserLogEnum.NORMAL,
                    User = user!,
                });

                return View(movieShowtimeAddToCartRequest);
            }
            catch
            {
                await userLogService.CreateAsync(new()
                {
                    Message = $"User POST TicketController.ConfirmOrder(...) failed",
                    Type = UserLogEnum.NORMAL,
                    User = user!,
                });
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        [HttpGet("/ticket/success/")]
        public async Task<ActionResult> Success([FromQuery] string id)
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
                await userLogService.CreateAsync(new()
                {
                    Message = $"User POST TicketController.Success(Id={id}) failed",
                    Type = UserLogEnum.ERROR,
                    User = user!,
                });

                return RedirectToAction(nameof(Cancel), "Ticket");
            }

            await userLogService.CreateAsync(new()
            {
                Message = $"User POST TicketController.Success(Id={id})",
                Type = UserLogEnum.NORMAL,
                User = user!,
            });

            ViewBag.MovieShowtime = movieShowtime;
            return View(ticket);
        }

        [HttpGet("/ticket/cancel/")]
        public async Task<ActionResult> Cancel()
        {
            var user = HttpContext.Items["User"] as User;
            await userLogService.CreateAsync(new()
            {
                Message = $"User POST TicketController.Cancel()",
                Type = UserLogEnum.WARNING,
                User = user!,
            });

            return View();
        }
    }
}
