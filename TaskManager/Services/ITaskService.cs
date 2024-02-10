using TaskManager.DTOs;

namespace TaskManager.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskDTO>> GetAllTasks();
        Task<TaskDTO> GetTaskDetails(string id);
        Task<bool> InsertTask(TaskDTO task);
        Task<bool> UpdateTask(TaskDTO task);
        Task<bool> DeleteTask(string id);
        Task<bool> SaveTask(TaskDTO task);
    }
}
