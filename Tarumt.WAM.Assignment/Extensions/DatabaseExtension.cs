using Microsoft.EntityFrameworkCore;
using Tarumt.WAM.Assignment.Infrastructure.Context;

namespace Tarumt.WAM.Assignment.Extensions
{
    public static class DatabaseExtension
    {
        public static void AddDatabaseConfig(this IServiceCollection services, IConfiguration config, IWebHostEnvironment env)
        {
            services.AddDbContextPool<DatabaseContext>(options =>
            {
                string sqlServerConnectionString = $"Server=(LocalDB)\\MSSQLLocalDB;Database=AssignmentDB;AttachDbFilename={env.ContentRootPath}\\AssignmentDB.mdf;Trusted_Connection=True;";
                options.UseSqlServer(sqlServerConnectionString);
            });
        }
    }
}
