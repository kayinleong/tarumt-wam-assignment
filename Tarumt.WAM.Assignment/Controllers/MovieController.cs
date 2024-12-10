using Microsoft.AspNetCore.Mvc;
using Tarumt.WAM.Assignment.Infrastructure.Services;

namespace Tarumt.WAM.Assignment.Controllers
{
    public class MovieController(MovieService movieService) : Controller
    {
        [HttpGet("/movies/")]
        public ActionResult Index(int pageNumber = 1, int pageSize = 9, string keyword = "")
        {
            ViewBag.Keyword = keyword;

            try
            {
                var movie = movieService.GetAll(pageNumber, pageSize, keyword);
                return View(movie);
            }
            catch
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        [HttpGet("/movies/{id}/")]
        public async Task<ActionResult> GetById(string id)
        {
            try
            {
                var movie = await movieService.GetByIdAsync(id);
                return View(movie);
            }
            catch
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }
    }
}
