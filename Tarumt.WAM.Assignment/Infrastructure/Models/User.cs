using System.ComponentModel.DataAnnotations;

namespace Tarumt.WAM.Assignment.Infrastructure.Models
{
    public class User : BaseModel
    {
        [Required]
        public required string FirstName { get; set; }

        [Required]
        public required string LastName { get; set; }

        [Required]
        public required string Username { get; set; }

        [Required]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }

        [Required]
        public required UserSecurityMeta SecurityMeta { get; set; }

        public List<Ticket> Ticket { get; set; } = [];

        public string FullName => $"{FirstName} {LastName}";
    }
}
