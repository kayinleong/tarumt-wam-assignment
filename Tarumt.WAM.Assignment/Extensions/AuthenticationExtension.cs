using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Tarumt.WAM.Assignment.Infrastructure.Services;

namespace Tarumt.WAM.Assignment.Extensions
{
    public static class AuthenticationExtension
    {
        public static void AddAuthenticationConfig(this IServiceCollection services, IConfiguration config, IWebHostEnvironment env)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    bool cookieLifetimeSuccess = int.TryParse(config["Cookie:CookieLifetime"], out int cookieLifetime);
                    if (!cookieLifetimeSuccess)
                    {
                        throw new InvalidOperationException();
                    }

                    options.ExpireTimeSpan = TimeSpan.FromMilliseconds(cookieLifetime);
                    options.Cookie.MaxAge = options.ExpireTimeSpan;
                    options.Cookie.Name = "token";
                    options.LoginPath = "/account/login";
                    options.LogoutPath = "/account/logout";
                    options.ReturnUrlParameter = "returnUrl";

                    options.Events = new CookieAuthenticationEvents
                    {
                        OnValidatePrincipal = async context =>
                        {
                            var userRepository = context.HttpContext.RequestServices.GetRequiredService<UserService>();
                            var userId = context.Principal!.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                            var securityStamp = context.Principal.FindFirst("SecurityStamp")?.Value;

                            var user = await userRepository.GetByIdAsync(userId!);
                            if (user == null || user.SecurityStamps != securityStamp)
                            {
                                context.RejectPrincipal();
                                await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                            }
                        }
                    };
                });
        }
    }
}
