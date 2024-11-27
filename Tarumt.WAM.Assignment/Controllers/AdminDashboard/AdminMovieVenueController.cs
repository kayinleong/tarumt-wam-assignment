using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tarumt.WAM.Assignment.Infrastructure.Models;
using Tarumt.WAM.Assignment.Infrastructure.Services;

namespace Tarumt.WAM.Assignment.Controllers.AdminDashboard
{
    [Authorize]
    public class AdminMovieVenueController(MovieVenueService movieVenueService) : Controller
    {
        [HttpGet("/admin/movie_venues/")]
        public ActionResult Index(int pageNumber = 1, int pageSize = 10, string keyword = "")
        {
            var movieVenues = movieVenueService.GetAllAsync(pageNumber, pageSize, keyword);

            return View(movieVenues);
        }

        [HttpGet("/admin/movie_venues/create/")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpGet("/admin/movie_venues/{id}/")]
        public async Task<ActionResult> Detail(string id)
        {
            var movieVenue = await movieVenueService.GetByIdAsync(id);
            return View(movieVenue);
        }

        [HttpGet("/admin/movie_venues/{id}/delete/")]
        public async Task<ActionResult> Delete(string id)
        {
            var movieVenue = await movieVenueService.GetByIdAsync(id);
            try
            {
                await movieVenueService.DeleteByIdAsync(movieVenue);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessages = e.Message;
            }

            return RedirectToAction(nameof(Index), "AdminMovieVenue");
        }

        [HttpPost("/admin/movie_venues/create/")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(MovieVenue movieVenue)
        {
            if (!ModelState.IsValid)
            {
                return View(movieVenue);
            }

            try
            {
                await movieVenueService.CreateAsync(movieVenue);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessages = e.Message;
                return View(movieVenue);
            }

            return RedirectToAction(nameof(Index), "AdminMovieVenue");
        }

        [HttpPost("/admin/movie_venues/{id}/")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Detail(string id, MovieVenue movieVenue)
        {
            if (!ModelState.IsValid)
            {
                return View(movieVenue);
            }

            movieVenue.Id = id;
            try
            {
                await movieVenueService.UpdateByIdAsync(movieVenue);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessages = e.Message;
                return View(movieVenue);
            }

            return RedirectToAction(nameof(Index), "AdminMovieVenue", new { id });
        }
    }
}
