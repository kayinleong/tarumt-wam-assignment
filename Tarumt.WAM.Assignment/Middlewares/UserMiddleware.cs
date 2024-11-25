using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Tarumt.WAM.Assignment.Infrastructure.Services;
using Tarumt.WAM.Assignment.Infrastructure.Models;

namespace Tarumt.WAM.Assignment.Middlewares
{
    public static class UserMiddlewareExtension
    {
        public static IApplicationBuilder UseUserMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<UserMiddleware>();
        }
    }

    public class UserMiddleware
    {
        private readonly RequestDelegate _requestDelegate;

        public UserMiddleware(RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate;
        }

        public async Task InvokeAsync(HttpContext httpContext, UserService userService)
        {
            if (httpContext.Request.Path.Value!.StartsWith("/api/token/"))
            {
                await _requestDelegate(httpContext);
                return;
            }

            try
            {
                AuthenticateResult authenticateResult = await httpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                if (authenticateResult != null && authenticateResult.Succeeded)
                {
                    ClaimsIdentity identity = (authenticateResult.Principal.Identity as ClaimsIdentity)!;
                    Claim userId = authenticateResult.Principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)!;
                    User user = await userService.GetByIdAsync(userId.Value);

                    if (user == null)
                    {
                        await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                        httpContext.Response.Redirect("/account/login");
                        return;
                    }

                    httpContext.Items.Add("User", user);
                    httpContext.Items.Add("UserTickets", user.Tickets);
                    httpContext.Items.Add("UserSecurityMeta", user.SecurityMeta);
                }
            }
            catch { }

            await _requestDelegate(httpContext);
        }
    }
}
