using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TaskManager.Models;

namespace TaskManager.DTOs
{
    public class TaskDTO
    {
        public string TaskId { get; set; }

        public string UserId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string StateId { get; set; }

        public ApplicationUser User { get; set; }

        public TaskState TaskState { get; set; }
    }
}
