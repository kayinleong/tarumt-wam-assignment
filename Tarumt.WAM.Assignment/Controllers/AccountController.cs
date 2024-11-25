using Microsoft.AspNetCore.Mvc;
using Tarumt.WAM.Assignment.Infrastructure.Models;
using Tarumt.WAM.Assignment.Infrastructure.Requests;
using Tarumt.WAM.Assignment.Infrastructure.Services;

namespace Tarumt.WAM.Assignment.Controllers
{
    [Route("/account/{action}")]
    public class AccountController(UserService userService) : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost("login")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(UserLoginRequest userLoginRequest)
        {
            if (!ModelState.IsValid)
            {
                userLoginRequest.Password = string.Empty;
                return View(userLoginRequest);
            }

            try
            {
                var existingUser = await userService.GetByUsernameAsync(userLoginRequest.Username);
                userService.ValidatePassword(userLoginRequest.Password, existingUser);
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessages = e.Message;
                userLoginRequest.Password = string.Empty;
                return View(userLoginRequest);
            }
        }

        [HttpPost("register")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(UserCreateRequest userCreateRequest)
        {
            if (!string.IsNullOrEmpty(userCreateRequest.Password) && userCreateRequest.Password.Length <= 7)
            {
                ModelState.AddModelError(nameof(UserCreateRequest.Password), "Password must be longer than 7 characters.");
            }

            if (!string.IsNullOrEmpty(userCreateRequest.Password) &&  !userCreateRequest.Password.Any(char.IsLetter))
            {
                ModelState.AddModelError(nameof(UserCreateRequest.Password), "Password must contains at least one character.");
            }

            if (!string.IsNullOrEmpty(userCreateRequest.Password) && !userCreateRequest.Password.Any(char.IsDigit))
            {
                ModelState.AddModelError(nameof(UserCreateRequest.Password), "Password must contains at least one digit.");
            }

            if (!ModelState.IsValid)
            {
                userCreateRequest.Password = string.Empty;
                return View(userCreateRequest);
            }

            try
            {
                await userService.CreateAsync((User) userCreateRequest);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessages = e.Message;
                userCreateRequest.Password = string.Empty;
                return View(userCreateRequest);
            }

            return RedirectToAction(nameof(Login), "Account");
        }
    }
}
