namespace Tarumt.WAM.Assignment.Infrastructure.Models
{
    public class MovieVenueOngoingShowtimes
    {
        public required string MovieVenueId { get; set; }

        public required string VenueLocation { get; set; }

        public int OngoingShowtimes { get; set; }
    }
}
