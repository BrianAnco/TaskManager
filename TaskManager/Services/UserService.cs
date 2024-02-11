using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TaskManager.Data;
using TaskManager.DTOs;
using TaskManager.Models;
using TaskManager.Responses;

namespace TaskManager.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UserService(AppDbContext context) { _context = context; }

        public async Task<bool> DeleteUser(string id)
        {
            var user = await _context.ApplicationUsers.FindAsync(id);
            if (user == null) return false;

            _context.ApplicationUsers.Remove(user);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsers()
        {
            return await _context.ApplicationUsers
               .Select(user => new UserDTO
               {
                   ID = user.Id, Name = user.Name, Email = user.Email, UserName = user.UserName
               })
               .ToListAsync();
        }

        public async Task<UserDTO> GetUserDetails(string id)
        {
            var user = await _context.ApplicationUsers
                .Where(t => t.Id == id)
                .Select(user => new UserDTO
                {
                    ID = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    UserName = user.UserName
                })
                .FirstOrDefaultAsync();

            return user;
        }

        public async Task<bool> InsertUser(UserDTO userDto)
        {
            var user = new Models.ApplicationUser
            {
                Name = userDto.Name,
                UserName = userDto.Email,
                PasswordHash = userDto.Password,
                Email = userDto.Email
            };

            _context.ApplicationUsers.Add(user);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> SaveUser(UserDTO userDto)
        {
            if (userDto.ID.IsNullOrEmpty())
            {
                return await InsertUser(userDto);
            }
            else
            {
                return await UpdateUser(userDto);
            }
        }
        public async Task<bool> UpdateUser(UserDTO userDto)
        {
            var user = await _context.ApplicationUsers.FindAsync(userDto.ID);
            if (user == null) return false;

            user.Name = userDto.Name;
            user.UserName = userDto.Email;
            user.PasswordHash = userDto.Password;
            user.Email = userDto.Email;

            _context.ApplicationUsers.Update(user);
            return await _context.SaveChangesAsync() > 0;
        }
    } 
}
