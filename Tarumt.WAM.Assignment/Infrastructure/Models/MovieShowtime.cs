using System.ComponentModel.DataAnnotations;
using Tarumt.WAM.Assignment.Infrastructure.Constants;

namespace Tarumt.WAM.Assignment.Infrastructure.Models
{
    public class MovieShowtime : BaseModel
    {
        [Required]
        public required string Name { get; set; }

        [Required]
        public required string Description { get; set; }

        [Required]
        public required decimal Price { get; set; }

        [Required]
        public required decimal DiscountRate { get; set; }

        [Required]
        public required DateTime StartTime { get; set; }

        [Required]
        public required DateTime EndTime { get; set; }

        [Required]
        public required string AvailableSeats { get; set; } = "[1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40]";

        [Required]
        public required MovieShowtimeEnum Status { get; set; } = MovieShowtimeEnum.AVAILABLE;

        [Required]
        public required Movie Movie { get; set; }

        public List<Ticket> Tickets { get; set; } = [];

        public decimal FinalPrice => Price - (Price * DiscountRate / 100);
    }
}
