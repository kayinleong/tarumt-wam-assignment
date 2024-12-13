using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tarumt.WAM.Assignment.Infrastructure.Constants;
using Tarumt.WAM.Assignment.Infrastructure.Context;
using Tarumt.WAM.Assignment.Infrastructure.Responses;
using Tarumt.WAM.Assignment.Infrastructure.Services;

namespace Tarumt.WAM.Assignment.Controllers.AdminDashboard
{
    [Authorize]
    public class AdminController(DatabaseContext databaseContext, TicketService ticketService) : Controller
    {
        [HttpGet("/admin/")]
        public ActionResult Index()
        {
            return View(new AdminResponse()
            {
                TotalPaidTickets = databaseContext.Tickets!.Where(m => m.Status == TicketEnum.PAID).Count(),
                TotalMovies = databaseContext.Movies!.Count(),
                TotalMovieShowtimes = databaseContext.MovieShowtimes!.Count(),
                TotalMovieVenues = databaseContext.MovieVenues!.Count(),
                TotalUsers = databaseContext.Users!.Count(),
                TicketsDailyCounts = ticketService.GetDailyCount()
            });
        }
    }
}
