using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tarumt.WAM.Assignment.Infrastructure.Models;
using Tarumt.WAM.Assignment.Infrastructure.Requests;
using Tarumt.WAM.Assignment.Infrastructure.Services;
using Tarumt.WAM.Assignment.Services;

namespace Tarumt.WAM.Assignment.Controllers.AdminDashboard
{
    [Authorize]
    public class AdminMovieController(
        FileService fileService, 
        MovieService movieService, 
        MovieVenueService movieVenueService) : Controller
    {
        [HttpGet("/admin/movies/")]
        public ActionResult Index(int pageNumber = 1, int pageSize = 10, string keyword = "")
        {
            var movies = movieService.GetAllAsync(pageNumber, pageSize, keyword);

            return View(movies);
        }

        [HttpGet("/admin/movies/create/")]
        public ActionResult Create()
        {
            ViewBag.MovieVenues = movieVenueService.GetAllAsync(1, 9999, string.Empty);
            return View();
        }

        [HttpGet("/admin/movies/{id}/")]
        public async Task<ActionResult> Detail(string id)
        {
            ViewBag.MovieVenues = movieVenueService.GetAllAsync(1, 9999, string.Empty);
            var movie = await movieService.GetByIdAsync(id);
            return View((MovieRequest) movie);
        }

        [HttpGet("/admin/movies/{id}/delete/")]
        public async Task<ActionResult> Delete(string id)
        {
            var movie = await movieService.GetByIdAsync(id);
            try
            {
                await movieService.DeleteByIdAsync(movie);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessages = e.Message;
            }

            return RedirectToAction(nameof(Index), "AdminMovie");
        }

        [HttpPost("/admin/movies/create/")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([FromForm] MovieRequest movieRequest)
        {
            if (movieRequest.Image == null)
            {
                ViewBag.MovieVenues = movieVenueService.GetAllAsync(1, 9999, string.Empty);
                ViewBag.ErrorMessages = "Please upload an image";
                return View(movieRequest);
            }

            if (!ModelState.IsValid)
            {
                ViewBag.MovieVenues = movieVenueService.GetAllAsync(1, 9999, string.Empty);
                return View(movieRequest);
            }

            Movie movie = movieRequest;

            try
            {
                var movieVenue = await movieVenueService.GetByIdAsync(movieRequest.MovieVenueId);
                movie.MovieVenue = movieVenue;
            }
            catch (Exception e)
            {
                ViewBag.MovieVenues = movieVenueService.GetAllAsync(1, 9999, string.Empty);
                ViewBag.ErrorMessages = e.Message;
                return View(movieRequest);
            }

            string filename = movieRequest.Image.FileName;
            string[] filenameArray = filename.Split(".");
            string uniqueId = Guid.NewGuid().ToString();
            string finalFileName = $"{uniqueId}_{filenameArray[0]}.{filenameArray[1]}";
            await fileService.UploadAsync(movieRequest.Image, finalFileName, "movie/");
            movie.ImageUrl = finalFileName;

            try
            {
                await movieService.CreateAsync(movie);
            }
            catch (Exception e)
            {
                ViewBag.MovieVenues = movieVenueService.GetAllAsync(1, 9999, string.Empty);
                ViewBag.ErrorMessages = e.Message;
                return View(movieRequest);
            }

            return RedirectToAction(nameof(Index), "AdminMovie");
        }

        [HttpPost("/admin/movies/{id}/")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Detail([FromRoute] string id, [FromForm] MovieRequest movieRequest)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.MovieVenues = movieVenueService.GetAllAsync(1, 9999, string.Empty);
                return View(movieRequest);
            }

            Movie movie = movieRequest;
            movie.Id = id;

            try
            {
                var movieVenue = await movieVenueService.GetByIdAsync(movieRequest.MovieVenueId);
                movie.MovieVenue = movieVenue;
            }
            catch (Exception e)
            {
                ViewBag.MovieVenues = movieVenueService.GetAllAsync(1, 9999, string.Empty);
                ViewBag.ErrorMessages = e.Message;
                return View(movieRequest);
            }

            if (movieRequest.Image != null)
            {
                string filename = movieRequest.Image.FileName;
                string[] filenameArray = filename.Split(".");
                string uniqueId = Guid.NewGuid().ToString();
                string finalFileName = $"{uniqueId}_{filenameArray[0]}.{filenameArray[1]}";
                await fileService.UploadAsync(movieRequest.Image, finalFileName, "movie/");
                movie.ImageUrl = finalFileName;
            }

            try
            {
                await movieService.UpdateByIdAsync(movie);
            }
            catch (Exception e)
            {
                ViewBag.MovieVenues = movieVenueService.GetAllAsync(1, 9999, string.Empty);
                ViewBag.ErrorMessages = e.Message;
                return View(movieRequest);
            }

            return RedirectToAction(nameof(Index), "AdminMovie", new { id });
        }
    }
}
