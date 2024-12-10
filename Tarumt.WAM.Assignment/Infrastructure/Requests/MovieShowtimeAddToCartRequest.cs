using System.ComponentModel.DataAnnotations;

namespace Tarumt.WAM.Assignment.Infrastructure.Requests
{
    public class MovieShowtimeAddToCartRequest
    {
        public string? TicketId { get; set; }

        [Required]
        public required string MovieShowtimeId { get; set; }

        [Required]
        public required string SelectedSeat { get; set; }
    }
}
