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

        public DbSet<MovieVenueOngoingShowtime> MovieVenueOngoingShowtimes { get; set; }

        public DbSet<MovieShowtimeTicketsSold> MovieShowtimeTicketsSold { get; set; }

        public DbSet<MoviesSoldOutTicket> MoviesSoldOutTickets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TicketsDailyCount>(eb =>
            {
                eb.HasNoKey();
                eb.ToView("TicketsDailyCount");
            });

            modelBuilder.Entity<MovieVenueOngoingShowtime>(eb =>
            {
                eb.HasNoKey();
                eb.ToView("MovieVenueOngoingShowtimes");
            });

            modelBuilder.Entity<MovieShowtimeTicketsSold>(eb =>
            {
                eb.HasNoKey();
                eb.ToView("MovieShowtimeTicketsSold");
            });

            modelBuilder.Entity<MoviesSoldOutTicket>(eb =>
            {
                eb.HasNoKey();
                eb.ToView("MoviesSoldOutTickets");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
