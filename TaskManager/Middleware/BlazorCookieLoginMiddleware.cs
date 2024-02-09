using Microsoft.AspNetCore.Identity;
using System.Collections.Concurrent;
using TaskManager.DTOs;
using TaskManager.Models;

namespace TaskManager.Middleware
{
    public class LoginInfo
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }

    public class BlazorCookieLoginMiddleware
    {
        public static IDictionary<Guid, LoginDTO> Logins { get; private set; }
            = new ConcurrentDictionary<Guid, LoginDTO>();


        private readonly RequestDelegate _next;

        public BlazorCookieLoginMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async System.Threading.Tasks.Task Invoke(HttpContext context, SignInManager<ApplicationUser> signInMgr)
        {
            if (context.Request.Path == "/login" && context.Request.Query.ContainsKey("key"))
            {
                var key = Guid.Parse(context.Request.Query["key"]);
                var info = Logins[key];

                var result = await signInMgr.PasswordSignInAsync(info.Email, info.Password, false, lockoutOnFailure: true);
                info.Password = null;
                if (result.Succeeded)
                {
                    Logins.Remove(key);
                    context.Response.Redirect("/home");
                    return;
                }
                else if (result.RequiresTwoFactor)
                {
                    //TODO: redirect to 2FA razor component
                    context.Response.Redirect("/loginwith2fa/" + key);
                    return;
                }
                else
                {
                    //TODO: Proper error handling
                    context.Response.Redirect("/loginfailed");
                    return;
                }
            }
            else if (context.Request.Path == "/logout")
            {
                await signInMgr.SignOutAsync(); // Cierra la sesión del usuario
                context.Response.Redirect("/login"); // Redirige al usuario a la página de inicio o de inicio de sesión
                return;
            }
            else
            {
                await _next.Invoke(context);
            }
        }
    }
}
