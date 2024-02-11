using TaskManager.DTOs;

namespace TaskManager.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetAllUsers();
        Task<UserDTO> GetUserDetails(string id);
        Task<bool> InsertUser(UserDTO userDto);
        Task<bool> UpdateUser(UserDTO userDto);
        Task<bool> DeleteUser(string id);
        Task<bool> SaveUser(UserDTO userDto);
    }
}
