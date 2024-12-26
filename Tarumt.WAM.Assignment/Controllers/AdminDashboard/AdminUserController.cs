using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tarumt.WAM.Assignment.Infrastructure.Constants;
using Tarumt.WAM.Assignment.Infrastructure.Models;
using Tarumt.WAM.Assignment.Infrastructure.Requests;
using Tarumt.WAM.Assignment.Infrastructure.Services;

namespace Tarumt.WAM.Assignment.Controllers.AdminDashboard
{
    [Authorize]
    public class AdminUserController(UserService userService, UserLogService userLogService) : Controller
    {
        [HttpGet("/admin/users/")]
        public ActionResult Index(int pageNumber = 1, int pageSize = 10, string keyword = "", UserEnum userStatus = UserEnum.ADMIN)
        {
            ViewBag.Status = userStatus;
            var users = userService.GetAllAsync(pageNumber, pageSize, keyword, userStatus);
            return View(users);
        }

        [HttpGet("/admin/users/{id}/logs/")]
        public ActionResult ViewLog(string id, int pageNumber = 1, int pageSize = 10)
        {
            var userLogs = userLogService.GetAllByUserId(id, pageNumber, pageSize);
            ViewBag.Id = id;
            return View(userLogs);
        }

        [HttpGet("/admin/users/{id}/")]
        public async Task<ActionResult> Detail(string id)
        {
            var user = await userService.GetByIdAsync(id);

            return View((UserRequest)user);
        }

        [HttpGet("/admin/users/{id}/block/")]
        public async Task<ActionResult> Block(string id)
        {
            User user = null;
            try
            {
                user = await userService.GetByIdAsync(id);
            }
            catch
            {
                return RedirectToAction(nameof(Index), "AdminUser");
            }

            try
            {
                user.LoginAttempt = 4;
                await userService.UpdateByIdAsync(user);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessages = e.Message;
                return RedirectToAction(nameof(Index), "AdminUser");
            }

            return RedirectToAction(nameof(Index), "AdminUser");
        }

        [HttpGet("/admin/users/{id}/unblock/")]
        public async Task<ActionResult> Unblock(string id)
        {
            User user = null;
            try
            {
                user = await userService.GetByIdAsync(id);
            }
            catch
            {
                return RedirectToAction(nameof(Index), "AdminUser");
            }

            try
            {
                user.LoginAttempt = 0;
                await userService.UpdateByIdAsync(user);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessages = e.Message;
                return RedirectToAction(nameof(Index), "AdminUser");
            }

            return RedirectToAction(nameof(Index), "AdminUser");
        }

        [HttpGet("/admin/users/{id}/delete/")]
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
