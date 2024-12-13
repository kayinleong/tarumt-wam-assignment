namespace Tarumt.WAM.Assignment.Infrastructure.Models
{
    public class MoviesSoldOutTickets
    {
        public required string MovieId { get; set; }

        public required string MovieName { get; set; }

        public int TicketsSold { get; set; }
    }
}
