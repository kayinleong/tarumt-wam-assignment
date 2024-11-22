using System.ComponentModel.DataAnnotations;
using Tarumt.WAM.Assignment.Infrastructure.Constants;

namespace Tarumt.WAM.Assignment.Infrastructure.Models
{
    public class UserSecurityMeta : BaseModel
    {
        [Required]
        public required UserEnum Type { get; set; } = UserEnum.GUEST;

        [Required]
        public required int LoginAttempt { get; set; } = 0;

        [Required]
        public required string SecurityStamps { get; set; }
    }
}
