using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tarumt.WAM.Assignment.Infrastructure.Constants;
using Tarumt.WAM.Assignment.Infrastructure.Requests;

namespace Tarumt.WAM.Assignment.Infrastructure.Models
{
    public class MovieShowtime : BaseModel
    {
        [Required]
        public required string Name { get; set; }

        [Required]
        public required string Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(6, 2)")]
        public required decimal Price { get; set; }

        [Required]
        [Column(TypeName = "decimal(6, 2)")]
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

        [Required]
        public required MovieVenue MovieVenue { get; set; }

        public List<Ticket> Tickets { get; set; } = [];

        public decimal FinalPrice => Price - (Price * DiscountRate / 100);

        public bool IsExpired => DateTime.Today > EndTime;

        public void BookAvailableSeats(string bookedSeatJson)
        {
            var bookedSeats = bookedSeatJson
                .Trim('[', ']', '"')
                .Replace("\",\"", ",")
                .Split(',')
                .Select(int.Parse)
                .ToList();

            var currentSeats = AvailableSeats
                .Trim('[', ']')
                .Split(',')
                .Select(int.Parse)
                .ToList();

            foreach (var seat in bookedSeats)
            {
                currentSeats.Remove(seat);
            }

            AvailableSeats = "[" + string.Join(", ", currentSeats) + "]";
        }

        public static implicit operator MovieShowtime(MovieShowtimeRequest movieShowtimeCreateRequest)
        {
            return new()
            {
                Name = movieShowtimeCreateRequest.Name,
                Description = movieShowtimeCreateRequest.Description,
                Price = movieShowtimeCreateRequest.Price,
                DiscountRate = movieShowtimeCreateRequest.DiscountRate,
                StartTime = movieShowtimeCreateRequest.StartTime,
                EndTime = movieShowtimeCreateRequest.EndTime,
                AvailableSeats = "[1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40]",
                Status = MovieShowtimeEnum.AVAILABLE,
                Movie = null,
                MovieVenue = null
            };
        }

        public static explicit operator MovieShowtimeRequest(MovieShowtime movieShowtime)
        {
            return new()
            {
                Id = movieShowtime.Id,
                Name = movieShowtime.Name,
                Description = movieShowtime.Description,
                Price = movieShowtime.Price,
                DiscountRate = movieShowtime.DiscountRate,
                StartTime = movieShowtime.StartTime,
                EndTime = movieShowtime.EndTime,
                AvailableSeats = movieShowtime.AvailableSeats,
                Status = movieShowtime.Status,
                MovieId = movieShowtime.Movie.Id,
                MovieVenueId = movieShowtime.MovieVenue.Id
            };
        }
    }
}
