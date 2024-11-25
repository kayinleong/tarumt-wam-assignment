using Microsoft.AspNetCore.Identity;
using Tarumt.WAM.Assignment.Infrastructure.HostedService;
using Tarumt.WAM.Assignment.Infrastructure.Models;
using Tarumt.WAM.Assignment.Infrastructure.Services;

namespace Tarumt.WAM.Assignment.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddServiceConfig(this IServiceCollection services)
        {
            services.AddHostedService<DatabaseHostedService>();

            services.AddScoped<UserService>();
            services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();
        }
    }
}
