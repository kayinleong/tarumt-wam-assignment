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
        public required UserEnum Type { get; set; } = UserEnum.GUEST;

        [Required]
        public required int LoginAttempt { get; set; } = 0;

        [Required]
        public required string SecurityStamps { get; set; }

        public List<Ticket> Tickets { get; set; } = [];

        public List<UserLog> UserLogs { get; set; } = [];

        public string FullName => $"{FirstName} {LastName}";

        public bool HasPendingTicket => Tickets.Any(ticket => ticket.Status == TicketEnum.PENDING);

        public void GenerateSecurityStamps()
        {
            SecurityStamps = Guid.NewGuid().ToString();
        }

        public static implicit operator User(UserCreateRequest userCreateRequest)
        {
            return new()
            {
                FirstName = userCreateRequest.FirstName,
                LastName = userCreateRequest.LastName,
                Username = userCreateRequest.Username,
                Email = userCreateRequest.Email,
                Password = userCreateRequest.Password,
                Type = UserEnum.GUEST,
                LoginAttempt = 0,
                SecurityStamps = string.Empty,
            };
        }

        public static implicit operator User(UserRequest userRequest)
        {
            return new()
            {
                Id = userRequest.Id,
                FirstName = userRequest.FirstName,
                LastName = userRequest.LastName,
                Username = userRequest.Username,
                Email = userRequest.Email,
                Password = string.Empty,
                Type = userRequest.Type,
                LoginAttempt = 0,
                SecurityStamps = string.Empty,
            };
        }

        public static explicit operator UserRequest(User user)
        {
            return new()
            {
                Id = user.Id,
                LoginAttempt = user.LoginAttempt,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.Username,
                Email = user.Email,
                Type = user.Type
            };
        }
    }
}
