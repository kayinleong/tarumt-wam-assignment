using System.ComponentModel.DataAnnotations;

namespace Tarumt.WAM.Assignment.Infrastructure.Models
{
    public class Movie : BaseModel
    {
        [Required]
        public required string Name { get; set; }

        [Required]
        public required string Description { get; set; }

        [Required]
        public required string ImageUrl { get; set; }

        [Required]
        public required MovieVenue MovieVenue { get; set; }

        public List<MovieShowtime> MovieShowtimes { get; set; } = [];
    }
}
