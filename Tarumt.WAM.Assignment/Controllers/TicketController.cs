using Microsoft.AspNetCore.Mvc;
using Tarumt.WAM.Assignment.Infrastructure.Models;
using Tarumt.WAM.Assignment.Infrastructure.Requests;
using Tarumt.WAM.Assignment.Infrastructure.Services;
using Microsoft.Extensions.Options;

namespace Tarumt.WAM.Assignment.Controllers
{
    public class TicketController(MovieShowtimeService movieShowtimeService, TicketService ticketService, IOptions<StripeSettings> stripeSettings) : Controller
    {
        [HttpGet("/ticket/confirm_order/")]
        public async Task<ActionResult> ConfirmOrder([FromQuery] MovieShowtimeAddToCartRequest movieShowtimeAddToCartRequest)
        {
            try
            {
                ViewBag.MovieShowtime = await movieShowtimeService.GetByIdAsync(movieShowtimeAddToCartRequest.Id);
                ViewBag.PublishableKey = stripeSettings.Value.PublishableKey;
                return View(movieShowtimeAddToCartRequest);
            }
            catch
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        [HttpGet("/ticket/success/")]
        public IActionResult Success([FromQuery] MovieShowtimeAddToCartRequest movieShowtimeAddToCartRequest)
        {
            return View();
        }

        [HttpGet("/ticket/cancel/")]
        public IActionResult Cancel()
        {
            return View();
        }
    }
}
