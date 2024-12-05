using Microsoft.AspNetCore.Authentication.Cookies;

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
                });
        }
    }
}
