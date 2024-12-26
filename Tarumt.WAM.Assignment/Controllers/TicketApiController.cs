using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using Stripe;
using Tarumt.WAM.Assignment.Infrastructure.Requests;
using Tarumt.WAM.Assignment.Infrastructure.Services;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Tarumt.WAM.Assignment.Infrastructure.Models;
using Tarumt.WAM.Assignment.Infrastructure.Constants;

namespace Tarumt.WAM.Assignment.Controllers
{
    [Authorize]
    [ApiController]
    public class TicketApiController(UserLogService userLogService, MovieShowtimeService movieShowtimeService) : ControllerBase
    {
        [HttpPost("/ticket/confirm_order/")]
        public async Task<IActionResult> CreateCheckoutSession(MovieShowtimeAddToCartRequest movieShowtimeAddToCartRequest)
        {
            User user = (User)HttpContext.Items["User"]!;
            var movieShowtime = await movieShowtimeService.GetByIdAsync(movieShowtimeAddToCartRequest.MovieShowtimeId);
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
                SuccessUrl = Url.Action(nameof(TicketController.Success), "Ticket", new { Id = movieShowtimeAddToCartRequest.TicketId }, Request.Scheme),
                CancelUrl = Url.Action(nameof(TicketController.Success), "Ticket", null, Request.Scheme),
            };

            try
            {
                var service = new SessionService();
                Session session = await service.CreateAsync(options);

                await userLogService.CreateAsync(new()
                {
                    Message = $"User POST TicketApiController.CreateCheckoutSession(...)",
                    Type = UserLogEnum.NORMAL,
                    User = user,
                });

                return Ok(new { id = session.Id });
            }
            catch (StripeException e)
            {
                await userLogService.CreateAsync(new()
                {
                    Message = $"User POST TicketApiController.CreateCheckoutSession(...) failed",
                    Type = UserLogEnum.ERROR,
                    User = user,
                });
                return BadRequest(new { error = e.Message });
            }
        }
    }
}
