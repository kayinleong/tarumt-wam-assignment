using Microsoft.EntityFrameworkCore;
using Tarumt.WAM.Assignment.Infrastructure.Models;

namespace Tarumt.WAM.Assignment.Infrastructure.Context
{
    public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
    {
        public DbSet<User>? Users { get; set; }

        public DbSet<Movie>? Movies { get; set; }

        public DbSet<MovieShowtime>? MovieShowtimes { get; set; }

        public DbSet<MovieVenue>? MovieVenues { get; set; }

        public DbSet<Ticket>? Tickets { get; set; }

        public DbSet<TicketsDailyCount> TicketsDailyCounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TicketsDailyCount>(eb =>
            {
                eb.HasNoKey();
                eb.ToView("TicketsDailyCount");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
