using TaskManager.DTOs;
using TaskManager.Responses;

namespace TaskManager.Services
{
    public interface IAccount
    {
        Task<ServiceResponse> RegisterAsync(RegisterDTO model);
        Task<ServiceResponse> LoginAsync(LoginDTO model);
    }
}
