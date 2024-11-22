using System.ComponentModel.DataAnnotations;
using Tarumt.WAM.Assignment.Infrastructure.Constants;

namespace Tarumt.WAM.Assignment.Infrastructure.Models
{
    public class Ticket : BaseModel
    {
        [Required]
        public required string SeatNumbers { get; set; } = "[]";

        [Required]
        public required MovieShowtime MovieShowtime { get; set; }

        [Required]
        public required TicketEnum Status { get; set; } = TicketEnum.PENDING;

        [Required]
        public required User User { get; set; }
    }
}
