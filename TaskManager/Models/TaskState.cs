using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Models
{
    public class TaskState
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string StateId { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string StateName { get; set; }

        // Navigation property
        public virtual ICollection<Task> Tasks { get; set; }
    }
}