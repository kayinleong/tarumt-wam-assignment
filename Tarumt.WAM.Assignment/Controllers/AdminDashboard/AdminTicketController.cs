using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tarumt.WAM.Assignment.Infrastructure.Models;
using Tarumt.WAM.Assignment.Infrastructure.Services;

namespace Tarumt.WAM.Assignment.Controllers.AdminDashboard
{
    [Authorize]
    public class AdminTicketController(TicketService ticketService) : Controller
    {
        [HttpGet("/admin/ticket/")]
        public ActionResult Index(int pageNumber = 1, int pageSize = 10)
        {
            var tickets = ticketService.GetAllByStatusAsync(pageNumber, pageSize, string.Empty);

            return View(tickets);
        }

        [HttpGet("/admin/ticket/{id}")]
        public async Task<ActionResult> Detail(string id)
        {
            var ticket = await ticketService.GetByIdAsync(id);
            return View(ticket);
        }

        [HttpGet("/admin/ticket/validate")]
        public ActionResult Validate()
        {
            return View();
        }

        [HttpGet("/admin/ticket/validate/result/fail")]
        public ActionResult ValidateFailed()
        {
            return View();
        }

        [HttpGet("/admin/ticket/validate/result")]
        public async Task<ActionResult> ValidateResult(string ticketId, string seats)
        {
            try
            {
                var ticket = await ticketService.GetByIdAsync(ticketId);
                if (ticket.SeatNumbers != seats)
                {
                    return RedirectToAction(nameof(ValidateFailed), "AdminTicket");
                }

                return View(ticket);
            }
            catch
            {
                return RedirectToAction(nameof(ValidateFailed), "AdminTicket");
            }
        }

        [HttpPost("/admin/ticket/{id}")]
        public async Task <ActionResult> Detail(string id, Ticket ticket)
        {
            if (!ModelState.IsValid)
            {
                return View(ticket);
            }

            ticket.Id = id;
            try
            {
                await ticketService.UpdateByIdAsync(ticket);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessages = e.Message;
                return View(ticket);
            }

            return RedirectToAction(nameof(Index), "AdminTicket", new { id });
        }
    }
}
