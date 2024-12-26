using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tarumt.WAM.Assignment.Infrastructure.Constants;
using Tarumt.WAM.Assignment.Infrastructure.Models;
using Tarumt.WAM.Assignment.Infrastructure.Services;

namespace Tarumt.WAM.Assignment.Controllers
{
    [Authorize]
    public class MovieShowtimeController(UserLogService userLogService, MovieShowtimeService movieShowtimeService) : Controller
    {
        [HttpGet("/movie_showtime/{id}/")]
        public async Task<ActionResult> GetById(string id)
        {
            User user = (User)HttpContext.Items["User"]!;
            await userLogService.CreateAsync(new()
            {
                Message = $"User GET MovieShowtimeController.GetById(Id={id})",
                Type = UserLogEnum.NORMAL,
                User = user,
            });

            try
            {
                ViewBag.MovieShowtime = await movieShowtimeService.GetByIdAsync(id);
                return View();
            }
            catch
            {
                await userLogService.CreateAsync(new()
                {
                    Message = $"User GET MovieShowtimeController.GetById(Id={id}) failed",
                    Type = UserLogEnum.ERROR,
                    User = user,
                });
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }
    }
}
