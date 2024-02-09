using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskManager.Models;

namespace TaskManager.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { } 

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        // DbSet para las tareas
        public DbSet<Models.Task> Tasks { get; set; }

        // DbSet para los estados de las tareas
        public DbSet<TaskState> TaskStates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración inicial de TaskStates con IDs de tipo string
            modelBuilder.Entity<TaskState>().HasData(
                new TaskState { StateId = Guid.NewGuid().ToString(), StateName = "Pendiente" },
                new TaskState { StateId = Guid.NewGuid().ToString(), StateName = "En Progreso" },
                new TaskState { StateId = Guid.NewGuid().ToString(), StateName = "Completada" }
            );

            // Configura las relaciones
            modelBuilder.Entity<Models.Task>()
                .HasOne<ApplicationUser>(t => t.User)
                .WithMany(u => u.Tasks)
                .HasForeignKey(t => t.UserId);

            modelBuilder.Entity<Models.Task>()
                .HasOne<TaskState>(t => t.TaskState)
                .WithMany(ts => ts.Tasks)
                .HasForeignKey(t => t.StateId);
        }
    }
}
