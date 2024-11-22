using Tarumt.WAM.Assignment.Infrastructure.HostedService;

namespace Tarumt.WAM.Assignment.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddServiceConfig(this IServiceCollection services)
        {
            services.AddHostedService<DatabaseHostedService>();
        }
    }
}
