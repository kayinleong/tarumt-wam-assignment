using System.ComponentModel.DataAnnotations;

namespace Tarumt.WAM.Assignment.Infrastructure.Requests
{
    public class MovieRequest
    {
        public string? Id { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public required string Description { get; set; }
        
        public IFormFile? Image { get; set; }

        [Required]
        [Display(Name = "Movie Venue")]
        public required string MovieVenueId { get; set; }
    }
}
