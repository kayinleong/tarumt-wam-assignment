using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Tarumt.WAM.Assignment.Infrastructure.Models;
using Tarumt.WAM.Assignment.Infrastructure.Requests;
using Tarumt.WAM.Assignment.Infrastructure.Services;

namespace Tarumt.WAM.Assignment.Controllers
{
    public class AccountController(UserService userService) : Controller
    {
        [HttpGet("/account/login/")]
        public ActionResult Login()
        {
            return View();
        }

        [HttpGet("/account/register/")]
        public ActionResult Register()
        {
            return View();
        }

        [HttpGet("/account/logout")]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpPost("/account/login/")]
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
                if (existingUser.SecurityMeta.LoginAttempt > 3)
                {
                    ViewBag.ErrorMessages = "You have exceeded login attempts, please submit a support ticket for help.";
                    userLoginRequest.Password = string.Empty;
                    return View(userLoginRequest);
                }

                await userService.ValidatePassword(userLoginRequest.Password, existingUser);

                var claimsIdentity = new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.NameIdentifier, existingUser.Id.ToString())
                ], CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessages = e.Message;
                userLoginRequest.Password = string.Empty;
                return View(userLoginRequest);
            }
        }

        [HttpPost("/account/register/")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(UserCreateRequest userCreateRequest)
        {
            if (!string.IsNullOrEmpty(userCreateRequest.Password) && userCreateRequest.Password.Length <= 7)
            {
                ModelState.AddModelError(nameof(UserCreateRequest.Password), "Password must be longer than 7 characters.");
            }

            if (!string.IsNullOrEmpty(userCreateRequest.Password) && !userCreateRequest.Password.Any(char.IsLetter))
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
                await userService.CreateAsync((User)userCreateRequest);
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
