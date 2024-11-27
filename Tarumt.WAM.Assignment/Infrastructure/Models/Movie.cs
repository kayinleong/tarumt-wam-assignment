using System.ComponentModel.DataAnnotations;
using Tarumt.WAM.Assignment.Infrastructure.Requests;

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

        public static implicit operator Movie(MovieRequest movieCreateRequest)
        {
            return new()
            {
                Name = movieCreateRequest.Name,
                Description = movieCreateRequest.Description,
                ImageUrl =  null,
                MovieVenue = null
            };
        }

        public static explicit operator MovieRequest(Movie movie)
        {
            return new()
            {
                Id = movie.Id,
                Name = movie.Name,
                Description = movie.Description,
                MovieVenueId = movie.MovieVenue.Id
            };
        }
    }
}
