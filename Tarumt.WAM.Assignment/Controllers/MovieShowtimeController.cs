using Microsoft.AspNetCore.Mvc;
using Tarumt.WAM.Assignment.Infrastructure.Services;

namespace Tarumt.WAM.Assignment.Controllers
{
    public class MovieShowtimeController(MovieShowtimeService movieShowtimeService) : Controller
    {
        [HttpGet("/movie_showtime/{id}/")]
        public async Task<ActionResult> GetById(string Id)
        {
            try
            {
                ViewBag.MovieShowtime = await movieShowtimeService.GetByIdAsync(Id);
                return View();
            }
            catch
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }
    }
}
