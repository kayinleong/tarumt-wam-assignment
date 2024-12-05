using System.ComponentModel.DataAnnotations;

namespace Tarumt.WAM.Assignment.Infrastructure.Requests
{
    public class MovieShowtimeAddToCartRequest
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public required string SelectedSeat { get; set; }
    }
}
