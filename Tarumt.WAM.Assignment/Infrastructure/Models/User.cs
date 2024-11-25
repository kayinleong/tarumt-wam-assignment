using System.ComponentModel.DataAnnotations;
using Tarumt.WAM.Assignment.Infrastructure.Constants;
using Tarumt.WAM.Assignment.Infrastructure.Requests;

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

        public List<Ticket> Tickets { get; set; } = [];

        public string FullName => $"{FirstName} {LastName}";

        public static implicit operator User(UserCreateRequest userCreateRequest)
        {
            return new()
            {
                FirstName = userCreateRequest.FirstName,
                LastName = userCreateRequest.LastName,
                Username = userCreateRequest.Username,
                Email = userCreateRequest.Email,
                Password = userCreateRequest.Password,
                SecurityMeta = new()
                {
                    Type = UserEnum.GUEST,
                    LoginAttempt = 0,
                    SecurityStamps = string.Empty,
                    CreatedAt = DateTime.Today,
                    UpdatedAt = DateTime.Today,
                },
                CreatedAt = DateTime.Today,
                UpdatedAt = DateTime.Today,
            };
        }
    }
}
