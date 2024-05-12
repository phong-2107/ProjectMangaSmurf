using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using ProjectMangaSmurf.Models;
using ProjectMangaSmurf.Areas.Common.Repository; // Ensure this namespace correctly references where IUserRepository is defined
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic; // Ensure this is added if you use List<>

namespace ProjectMangaSmurf.Areas.Common.Services
{
    public class CustomAuthenticationService
    {
        private readonly IUser userRepository; // Ensure this is IUserRepository
        private readonly IHttpContextAccessor httpContextAccessor;

        public CustomAuthenticationService(IUser userRepository, IHttpContextAccessor httpContextAccessor)
        {
            this.userRepository = userRepository;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> Login(string username, string password)
        {
            User user = userRepository.FindByUsername(username);
            if (user != null && VerifyPassword(password, user.Password)) // Ensure PasswordHash is used if that's your model's property
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, "UserRole") // Assuming role management
                };

                var claimsIdentity = new ClaimsIdentity(claims, "MyCookieAuth");
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                await httpContextAccessor.HttpContext.SignInAsync("MyCookieAuth", claimsPrincipal);

                return true;
            }
            return false;
        }

        private bool VerifyPassword(string providedPassword, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(providedPassword, storedHash);
        }

        public async Task Logout()
        {
            await httpContextAccessor.HttpContext.SignOutAsync("MyCookieAuth");
        }
    }
}
