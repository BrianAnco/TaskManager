using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TaskManager.Data;
using TaskManager.DTOs;
using TaskManager.Models;
using TaskManager.Responses;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;

namespace TaskManager.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(AppDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

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
                UserName = userDto.UserName,
                PasswordHash = userDto.CurrentPassword,
                Email = userDto.Email
            };

            var result = await _userManager.CreateAsync(user, userDto.CurrentPassword);

            if (result.Succeeded)
            {
                var userRole = await _roleManager.FindByNameAsync("User");
                if (userRole == null)
                {
                    await _roleManager.CreateAsync(new IdentityRole("User"));
                    await _userManager.AddToRoleAsync(user!, "User");

                    return true;

                }
                else
                {
                    await _userManager.AddToRoleAsync(user, "User");
                    return true;
                }



                await _userManager.AddToRoleAsync(user, "User");
                return true;
            }
            else
            {
                return false;
            }
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
            // Obtener el usuario usando UserManager en lugar de buscarlo directamente en el contexto de la base de datos.
            var user = await _userManager.FindByIdAsync(userDto.ID);
            if (user == null) return false;

            // Actualizar las propiedades del usuario según sea necesario.
            user.Name = userDto.Name;
            user.UserName = userDto.Email; // Asegúrate de que esto cumple con tu lógica de negocio.
            user.Email = userDto.Email;

            if (!string.IsNullOrEmpty(userDto.NewPassword))
            {
                var passwordChangeResult = await _userManager.ChangePasswordAsync(user, userDto.CurrentPassword, userDto.NewPassword);
            }

            // Aplicar los cambios usando UserManager.
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

    }
}
