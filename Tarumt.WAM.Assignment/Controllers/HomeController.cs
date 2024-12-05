using Microsoft.AspNetCore.Mvc;
using Tarumt.WAM.Assignment.Infrastructure.Services;

namespace Tarumt.WAM.Assignment.Controllers
{
    public class HomeController(MovieShowtimeService movieShowtimeService) : Controller
    {
        [HttpGet("/")]
        public ActionResult Index()
        {
            var movie = movieShowtimeService.GetTop6LatestAsync();

            return View(movie);
        }
    }
}
