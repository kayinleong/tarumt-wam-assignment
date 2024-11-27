﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tarumt.WAM.Assignment.Infrastructure.Context;
using Tarumt.WAM.Assignment.Infrastructure.Responses;

namespace Tarumt.WAM.Assignment.Controllers.AdminDashboard
{
    [Authorize]
    public class AdminController(DatabaseContext databaseContext) : Controller
    {
        [HttpGet("/admin/")]
        public ActionResult Index()
        {
            return View(new AdminResponse()
            {
                TotalMovies = databaseContext.Movies!.Count(),
                TotalMovieShowtimes = databaseContext.MovieShowtimes!.Count(),
                TotalMovieVenues = databaseContext.MovieVenues!.Count(),
                TotalUsers = databaseContext.Users!.Count()
            });
        }
    }
}