using Faregosoft.Api2.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Faregosoft.Api2.Helpers
{
    public interface IUserHelper
    {
        Task<User> GetUserAsync(string email);

        Task<IdentityResult> AddUserAsync(User user, string password);

        Task CheckRoleAsync(string roleName);

        Task AddUserToRoleAsync(User user, string roleName);

        Task<bool> IsUserInRoleAsync(User user, string roleName);

        Task LogoutAsync();

        Task<SignInResult> ValidatePasswordAsync(User user, string password);
    }
}
