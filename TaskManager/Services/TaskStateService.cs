using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.DTOs;
using TaskManager.Models;

namespace TaskManager.Services
{
    public class TaskStateService : ITaskStateService
    {
        private readonly AppDbContext _context;

        public TaskStateService(AppDbContext context) { _context = context; }

        public async Task<IEnumerable<TaskStateDTO>> GetAllTaskStates()
        {
            return await _context.TaskStates
                .Select(taskState => new TaskStateDTO
                {
                    StateId = taskState.StateId,
                    StateName = taskState.StateName
                })
                .ToListAsync();
        }

        public async Task<TaskStateDTO> GetTaskStateDetails(string id)
        {
            var taskState = await _context.TaskStates
                .Where(t => t.StateId == id)
                .Select(t => new TaskStateDTO
                {
                    StateId = t.StateId,
                    StateName = t.StateName
                })
                .FirstOrDefaultAsync();

            return taskState;
        }
    }
}
