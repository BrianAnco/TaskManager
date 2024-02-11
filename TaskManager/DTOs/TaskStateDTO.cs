using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TaskManager.DTOs
{
    public class TaskStateDTO
    {
        public string StateId { get; set; } = Guid.NewGuid().ToString();

        public string StateName { get; set; }

        public virtual ICollection<Task> Tasks { get; set; }
    }
}
