using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tarumt.WAM.Assignment.Infrastructure.Constants;
using Tarumt.WAM.Assignment.Infrastructure.Requests;
using Tarumt.WAM.Assignment.Infrastructure.Services;

namespace Tarumt.WAM.Assignment.Controllers.AdminDashboard
{
    [Authorize]
    public class AdminUserController(UserService userService) : Controller
    {
        [HttpGet("/admin/users/")]
        public ActionResult Index(int pageNumber = 1, int pageSize = 10, string keyword = "", UserEnum userStatus = UserEnum.ADMIN)
        {
            ViewBag.Status = userStatus;
            var users = userService.GetAllAsync(pageNumber, pageSize, keyword, userStatus);
            return View(users);
        }

        [HttpGet("/admin/users/{id}/")]
        public async Task<ActionResult> Detail(string id)
        {
            var user = await userService.GetByIdAsync(id);
            return View((UserRequest)user);
        }

        [HttpGet("/admin/movie_venues/{id}/delete/")]
        public async Task<ActionResult> Delete(string id)
        {
            var user = await userService.GetByIdAsync(id);
            try
            {
                await userService.DeleteByIdAsync(user);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessages = e.Message;
            }

            return RedirectToAction(nameof(Index), "AdminUser");
        }

        [HttpPost("/admin/users/{id}/")]
        public async Task<ActionResult> Detail(string id, UserRequest userRequest)
        {
            if (!ModelState.IsValid)
            {
                return View(userRequest);
            }

            userRequest.Id = id;
            try
            {
                await userService.UpdateByIdAsync(userRequest);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessages = e.Message;
                return View(userRequest);
            }

            return RedirectToAction(nameof(Index), "AdminUser");
        }
    }
}
