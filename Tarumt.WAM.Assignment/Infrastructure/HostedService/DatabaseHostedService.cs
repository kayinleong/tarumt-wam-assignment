using Microsoft.EntityFrameworkCore;
using Tarumt.WAM.Assignment.Infrastructure.Context;

namespace Tarumt.WAM.Assignment.Infrastructure.HostedService
{
    public class DatabaseHostedService : IHostedService
    {
        private readonly IServiceProvider _serverProvider;

        public DatabaseHostedService(IServiceProvider serverProvider)
        {
            _serverProvider = serverProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using IServiceScope scope = _serverProvider.CreateScope();

            DatabaseContext context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            await context.Database.MigrateAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
