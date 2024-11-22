using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Tarumt.WAM.Assignment.Infrastructure.Models
{
    public class BaseModel
    {
        [Required]
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; } = DateTime.Today;

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; } = DateTime.Today;
    }
}
