﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Models
{
    public class TaskState
    {
        [Key]
        public string StateId { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string StateName { get; set; }

        public virtual ICollection<Task> Tasks { get; set; }
    }
}