using Tarumt.WAM.Assignment.Infrastructure.Models;

namespace Tarumt.WAM.Assignment.Infrastructure.Responses
{
    public class AdminResponse
    {
        public int TotalPaidTickets { get; set; } = 0;

        public int TotalMovies { get; set; } = 0;

        public int TotalMovieShowtimes { get; set; } = 0;

        public int TotalMovieVenues { get; set; } = 0;

        public int TotalUsers { get; set; } = 0;

        public List<TicketsDailyCount> TicketsDailyCounts { get; set; }
    }
}
