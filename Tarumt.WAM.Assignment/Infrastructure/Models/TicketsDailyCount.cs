namespace Tarumt.WAM.Assignment.Infrastructure.Models
{
    public class TicketsDailyCount
    {
        public required DateTime TicketDate { get; set; }

        public int TotalTickets { get; set; }
    }
}
