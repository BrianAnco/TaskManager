using TaskManager.DTOs;

namespace TaskManager.Services
{
    public interface ITaskStateService
    {
        Task<IEnumerable<TaskStateDTO>> GetAllTaskStates();
        Task<TaskStateDTO> GetTaskStateDetails(string id);
    }
}
