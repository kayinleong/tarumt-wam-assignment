using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tarumt.WAM.Assignment.Infrastructure.Services;

namespace Tarumt.WAM.Assignment.Controllers.Worker
{
    [Authorize]
    public class WorkerTicketController(TicketService ticketService) : Controller
    {
        [HttpGet("/worker/ticket/validate")]
        public ActionResult Validate()
        {
            return View();
        }

        [HttpGet("/worker/ticket/validate/result/fail")]
        public ActionResult ValidateFailed()
        {
            return View();
        }

        [HttpGet("/worker/ticket/validate/result")]
        public async Task<ActionResult> ValidateResult(string ticketId, string seats)
        {
            try
            {
                var ticket = await ticketService.GetByIdAsync(ticketId);
                if (ticket.SeatNumbers != seats)
                {
                    return RedirectToAction(nameof(ValidateFailed), "WorkerTicket");
                }

                return View(ticket);
            }
            catch
            {
                return RedirectToAction(nameof(ValidateFailed), "WorkerTicket");
            }
        }
    }
}
