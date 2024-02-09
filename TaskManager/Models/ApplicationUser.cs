using Microsoft.AspNetCore.Identity;
using System.Collections.Generic; // Necesario para usar List<T>

namespace TaskManager.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; } = string.Empty;

        // Propiedad de navegación para las tareas asociadas a este usuario
        public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
    }
}
