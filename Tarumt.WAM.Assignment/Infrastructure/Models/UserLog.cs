using System.ComponentModel.DataAnnotations;
using Tarumt.WAM.Assignment.Infrastructure.Constants;

namespace Tarumt.WAM.Assignment.Infrastructure.Models
{
    public class UserLog : BaseModel
    {
        [Required]
        public required string Message { get; set; }

        [Required]
        public required UserLogEnum Type { get; set; } = UserLogEnum.NORMAL;

        [Required]
        public required User User { get; set; }
    }
}
