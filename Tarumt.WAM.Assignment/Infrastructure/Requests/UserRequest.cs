using System.ComponentModel.DataAnnotations;
using Tarumt.WAM.Assignment.Infrastructure.Constants;

namespace Tarumt.WAM.Assignment.Infrastructure.Requests
{
    public class UserRequest
    {
        public string? Id { get; set; }

        [Display(Name = "First Name")]
        [Required]
        public required string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required]
        public required string LastName { get; set; }

        [Required]
        public required string Username { get; set; }

        [Required]
        [RegularExpression(@"^\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z$", ErrorMessage = "Email is invalid")]
        public required string Email { get; set; }

        [Required]
        public required UserEnum Type { get; set; }
    }
}
