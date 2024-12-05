using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tarumt.WAM.Assignment.Infrastructure.Models;
using Tarumt.WAM.Assignment.Infrastructure.Requests;
using Tarumt.WAM.Assignment.Infrastructure.Services;

namespace Tarumt.WAM.Assignment.Controllers.AdminDashboard
{
    [Authorize]
    public class AdminMovieShowtimeController(
        MovieService movieService, 
        MovieVenueService movieVenueService, 
        MovieShowtimeService movieShowtimeService) : Controller
    {
        [HttpGet("/admin/movie_showtimes/")]
        public ActionResult Index(int pageNumber = 1, int pageSize = 10, string keyword = "")
        {
            var movieShowtimes = movieShowtimeService.GetAllAsync(pageNumber, pageSize, keyword);

            return View(movieShowtimes);
        }

        [HttpGet("/admin/movie_showtimes/create/")]
        public ActionResult Create()
        {
            ViewBag.Movies = movieService.GetAll(1, 9999, string.Empty);
            ViewBag.MovieVenues = movieVenueService.GetAllAsync(1, 9999, string.Empty);

            return View();
        }

        [HttpGet("/admin/movie_showtimes/{id}/")]
        public async Task<ActionResult> Detail(string id)
        {
            ViewBag.Movies = movieService.GetAll(1, 9999, string.Empty);
            ViewBag.MovieVenues = movieVenueService.GetAllAsync(1, 9999, string.Empty);
            var movieShowtime = await movieShowtimeService.GetByIdAsync(id);

            return View((MovieShowtimeRequest)movieShowtime);
        }

        [HttpGet("/admin/movie_showtimes/{id}/delete/")]
        public async Task<ActionResult> Delete(string id)
        {
            var movieShowtime = await movieShowtimeService.GetByIdAsync(id);
            try
            {
                await movieShowtimeService.DeleteByIdAsync(movieShowtime);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessages = e.Message;
            }

            return RedirectToAction(nameof(Index), "AdminMovieShowtime");
        }

        [HttpPost("/admin/movie_showtimes/create/")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(MovieShowtimeRequest movieShowtimeRequest)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Movies = movieService.GetAll(1, 9999, string.Empty);
                ViewBag.MovieVenues = movieVenueService.GetAllAsync(1, 9999, string.Empty);
                return View(movieShowtimeRequest);
            }

            MovieShowtime movieShowtime = movieShowtimeRequest;
            try
            {
                var movieVenue = await movieVenueService.GetByIdAsync(movieShowtimeRequest.MovieVenueId);
                movieShowtime.MovieVenue = movieVenue;
            }
            catch (Exception e)
            {
                ViewBag.MovieVenues = movieVenueService.GetAllAsync(1, 9999, string.Empty);
                ViewBag.ErrorMessages = e.Message;
                return View(movieShowtimeRequest);
            }

            try
            {
                var movie = await movieService.GetByIdAsync(movieShowtimeRequest.MovieId);
                movieShowtime.Movie = movie;
            }
            catch (Exception e)
            {
                ViewBag.Movies = movieService.GetAll(1, 9999, string.Empty);
                ViewBag.MovieVenues = movieVenueService.GetAllAsync(1, 9999, string.Empty);
                ViewBag.ErrorMessages = e.Message;
                return View(movieShowtimeRequest);
            }

            try
            {
                await movieShowtimeService.CreateAsync(movieShowtime);
            }
            catch (Exception e)
            {
                ViewBag.Movies = movieService.GetAll(1, 9999, string.Empty);
                ViewBag.MovieVenues = movieVenueService.GetAllAsync(1, 9999, string.Empty);
                ViewBag.ErrorMessages = e.Message;
                return View(movieShowtimeRequest);
            }

            return RedirectToAction(nameof(Index), "AdminMovieShowtime");
        }

        [HttpPost("/admin/movie_showtimes/{id}/")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Detail([FromRoute] string id, [FromForm] MovieShowtimeRequest movieShowtimeRequest)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Movies = movieService.GetAll(1, 9999, string.Empty);
                ViewBag.MovieVenues = movieVenueService.GetAllAsync(1, 9999, string.Empty);
                return View(movieShowtimeRequest);
            }

            MovieShowtime movieShowtime = movieShowtimeRequest;
            movieShowtime.Id = id;

            try
            {
                var movieVenue = await movieVenueService.GetByIdAsync(movieShowtimeRequest.MovieVenueId);
                movieShowtime.MovieVenue = movieVenue;
            }
            catch (Exception e)
            {
                ViewBag.MovieVenues = movieVenueService.GetAllAsync(1, 9999, string.Empty);
                ViewBag.ErrorMessages = e.Message;
                return View(movieShowtimeRequest);
            }

            try
            {
                var movie = await movieService.GetByIdAsync(movieShowtimeRequest.MovieId);
                movieShowtime.Movie = movie;
            }
            catch (Exception e)
            {
                ViewBag.Movies = movieService.GetAll(1, 9999, string.Empty);
                ViewBag.MovieVenues = movieVenueService.GetAllAsync(1, 9999, string.Empty);
                ViewBag.ErrorMessages = e.Message;
                return View(movieShowtimeRequest);
            }

            try
            {
                await movieShowtimeService.UpdateByIdAsync(movieShowtime);
            }
            catch (Exception e)
            {
                ViewBag.Movies = movieService.GetAll(1, 9999, string.Empty);
                ViewBag.MovieVenues = movieVenueService.GetAllAsync(1, 9999, string.Empty);
                ViewBag.ErrorMessages = e.Message;
                return View(movieShowtimeRequest);
            }

            return RedirectToAction(nameof(Index), "AdminMovieShowtime");
        }
    }
}
