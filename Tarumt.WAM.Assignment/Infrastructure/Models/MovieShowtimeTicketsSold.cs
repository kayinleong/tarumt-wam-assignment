namespace Tarumt.WAM.Assignment.Infrastructure.Models
{
    public class MovieShowtimeTicketsSold
    {
        public required string MovieShowtimeId { get; set; }

        public required string ShowtimeName { get; set; }

        public required string MovieId { get; set; }

        public required string MovieName { get; set; }

        public int TicketsSold { get; set; }
    }
}
