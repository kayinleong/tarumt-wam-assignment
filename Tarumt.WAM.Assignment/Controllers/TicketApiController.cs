using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using Stripe;
using Tarumt.WAM.Assignment.Infrastructure.Requests;
using Tarumt.WAM.Assignment.Infrastructure.Services;
using Microsoft.Extensions.Options;
using Tarumt.WAM.Assignment.Infrastructure.Models;
using System.Text.Json;

namespace Tarumt.WAM.Assignment.Controllers
{
    [ApiController]
    public class TicketApiController(MovieShowtimeService movieShowtimeService) : ControllerBase
    {
        [HttpPost("/ticket/confirm_order/")]
        public async Task<IActionResult> CreateCheckoutSession(MovieShowtimeAddToCartRequest movieShowtimeAddToCartRequest)
        {
            var movieShowtime = await movieShowtimeService.GetByIdAsync(movieShowtimeAddToCartRequest.Id);
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = ["card"],
                LineItems =
                [
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(JsonSerializer.Deserialize<List<string>>(movieShowtimeAddToCartRequest.SelectedSeat)!.Count * movieShowtime.FinalPrice * 100),
                            Currency = "myr",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = movieShowtime.Name,
                            },
                        },
                        Quantity = 1,
                    },
                ],
                Mode = "payment",
                SuccessUrl = Url.Action(nameof(TicketController.Success), "Ticket", movieShowtimeAddToCartRequest, Request.Scheme),
                CancelUrl = Url.Action(nameof(TicketController.Success), "Ticket", movieShowtimeAddToCartRequest, Request.Scheme),
            };

            try
            {
                var service = new SessionService();
                Session session = await service.CreateAsync(options);
                return Ok(new { id = session.Id });
            }
            catch (StripeException e)
            {
                return BadRequest(new { error = e.Message });
            }
        }
    }
}
