using TaskManager.Models;
using TaskManager.Data;
using Microsoft.EntityFrameworkCore;
using TaskManager.DTOs;
using Microsoft.IdentityModel.Tokens;

namespace TaskManager.Services
{
    public class TaskService : ITaskService
    {
        private readonly AppDbContext _context;

        public TaskService(AppDbContext context) { _context = context; }

        public async Task<bool> DeleteTask(string id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null) return false;

            _context.Tasks.Remove(task);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<TaskDTO>> GetAllTasks()
        {
            return await _context.Tasks
                .Include(t => t.User)
                .Include(t => t.TaskState)
                .Select(task => new TaskDTO
                {
                    TaskId = task.TaskId.ToString(),
                    UserId = task.UserId,
                    Title = task.Title,
                    Description = task.Description,
                    StateId = task.StateId.ToString(),
                    User = task.User,
                    TaskState = task.TaskState
                })
                .ToListAsync();
        }

        public async Task<TaskDTO> GetTaskDetails(string id)
        {
            var task = await _context.Tasks
                .Where(t => t.TaskId == id)
                .Include(t => t.User)
                .Include(t => t.TaskState)
                .Select(t => new TaskDTO
                {
                    TaskId = t.TaskId.ToString(),
                    UserId = t.UserId,
                    Title = t.Title,
                    Description = t.Description,
                    StateId = t.StateId.ToString(),
                    User = t.User,
                    TaskState = t.TaskState
                })
                .FirstOrDefaultAsync();

            return task;
        }


        public async Task<bool> InsertTask(TaskDTO taskDto)
        {
            var task = new Models.Task
            {
                TaskId = Guid.NewGuid().ToString(),
                UserId = taskDto.UserId,
                Title = taskDto.Title,
                Description = taskDto.Description,
                StateId = taskDto.StateId
            };

            _context.Tasks.Add(task);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> SaveTask(TaskDTO taskDto)
        {
            if (taskDto.TaskId.IsNullOrEmpty())
            {
                return await InsertTask(taskDto);
            }
            else
            {
                return await UpdateTask(taskDto);
            }
        }

        public async Task<bool> UpdateTask(TaskDTO taskDto)
        {
            var task = await _context.Tasks.FindAsync(taskDto.TaskId);
            if (task == null) return false;

            task.UserId = taskDto.UserId;
            task.Title = taskDto.Title;
            task.Description = taskDto.Description;
            task.StateId = taskDto.StateId;
            _context.Tasks.Update(task);
            return await _context.SaveChangesAsync() > 0;
        }

    }
}
