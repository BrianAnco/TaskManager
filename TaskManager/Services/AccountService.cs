using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;
using System.Security.Claims;
using TaskManager.DTOs;
using TaskManager.Models;
using TaskManager.Responses;

namespace TaskManager.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }
        
        public async Task<ServiceResponse> RegisterAsync(RegisterDTO RegisterDTO)
        {
            var findUser = await userManager.FindByEmailAsync(RegisterDTO.Email);
            if (findUser != null) return new ServiceResponse(false, "User already exist");

            var appUser = new ApplicationUser()
            {
                Name = RegisterDTO.Name,
                UserName = RegisterDTO.Email,
                PasswordHash = RegisterDTO.Password,
                Email = RegisterDTO.Email
            };
            
            var createUser = await userManager.CreateAsync(appUser, RegisterDTO.Password);
            if(createUser.Succeeded)
            {
                var adminRole = await roleManager.FindByNameAsync("Admin");
                if (adminRole == null)
                {
                    await roleManager.CreateAsync(new IdentityRole("Admin"));
                    await userManager.AddToRoleAsync(appUser!, "Admin");

                    return new ServiceResponse(true, "User created");

                }
                else
                {
                    var userRole = await roleManager.CreateAsync(new IdentityRole("User"));
                    if(userRole != null)
                    {
                        await userManager.AddToRoleAsync(appUser!, "User");
                    }

                    return new ServiceResponse(true, "User created");
                }
            }
            else
            {
                // Concatena los mensajes de error de IdentityResult
                var errors = string.Join("; ", createUser.Errors.Select(e => e.Description));
                return new ServiceResponse(false, $"Error occurred during user creation: {errors}");
            }

        }

        private static readonly Task<AuthenticationState> defaultUnauthenticatedTask =
            System.Threading.Tasks.Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));

        private readonly System.Threading.Tasks.Task<AuthenticationState> authenticationStateTask = defaultUnauthenticatedTask;

        public async Task<ServiceResponse> LoginAsync(LoginDTO loginDto)
        {
            var findUser = await userManager.FindByEmailAsync(loginDto.Email);
            if (findUser == null) return new ServiceResponse(false, "User not found.");
            
            var getRole = (await userManager.GetRolesAsync(findUser!)).FirstOrDefault();
            var claims = new List<Claim>()
            {
                new(ClaimTypes.Name, findUser!.Name!),
                new(ClaimTypes.Email, findUser!.Email!),
                new(ClaimTypes.Role, getRole!),
            };

            var result = await signInManager.CheckPasswordSignInAsync(findUser, loginDto.Password, false);
            if (!result.Succeeded)
                return new ServiceResponse(false, "Error Ocurred");


            Console.WriteLine($"Signing in user: {findUser.UserName}");
            foreach (var claim in claims)
            {
                Console.WriteLine($"Claim: {claim.Type} - {claim.Value}");
            }


            //authenticationStateTask = System.Threading.Tasks.Task.FromResult(
            //    new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims,
            //    authenticationType: nameof(Account)))));
             


            await signInManager.SignInWithClaimsAsync(findUser,false, claims);
            return new ServiceResponse(true, "Success");

        }
    }
}
