using Microsoft.AspNetCore.Mvc.Razor;
using Tarumt.WAM.Assignment.Infrastructure.Policies;

namespace Tarumt.WAM.Assignment.Extensions
{
    public static class MvcExtension
    {
        public static void AddMvcConfig(this IServiceCollection services, IConfiguration config, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                services.AddMvc()
                    .AddJsonOptions(options =>
                    {
                        options.JsonSerializerOptions.PropertyNamingPolicy = new SnakeCaseNamingPolicy();
                    })
                    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                    .AddDataAnnotationsLocalization()
                    .AddRazorRuntimeCompilation();
            }
            else
            {
                services.AddMvc()
                    .AddJsonOptions(options =>
                    {
                        options.JsonSerializerOptions.PropertyNamingPolicy = new SnakeCaseNamingPolicy();
                    })
                    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                    .AddDataAnnotationsLocalization();
            }

            services.Configure<RouteOptions>(options => options.AppendTrailingSlash = true);
        }
    }
}
