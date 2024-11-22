using System.ComponentModel.DataAnnotations;

namespace Tarumt.WAM.Assignment.Infrastructure.Models
{
    public class MovieVenue : BaseModel
    {
        [Required]
        public required string Location { get; set; }

        [Required]
        public required string Coordinate { get; set; }

        public List<MovieShowtime> MovieShowtimes { get; set; } = [];
    }
}
