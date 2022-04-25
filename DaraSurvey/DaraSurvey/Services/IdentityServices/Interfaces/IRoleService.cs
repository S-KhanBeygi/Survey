using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DaraSurvey.Interfaces
{
    public interface IRoleService
    {
        Task<IdentityResult> AddRolesToUserAsync(string userName, IEnumerable<string> roles);
        Task<IdentityResult> AddRoleToUserAsync(string userId, string roleId);
        Task<IdentityResult> AddToRolesAsync(string userId, IEnumerable<string> roles);
        Task<IdentityResult> CreateAsybc(string role);
        Task<IdentityResult> DeleteAsync(string roleName);
        IEnumerable<IdentityRole> GetAll();
        IEnumerable<string> GetUserIdsByRole(IEnumerable<string> roles);
        Task<IEnumerable<string>> GetUserRolesAsync(string userId);
        Task<IdentityResult> RemoveRolesFromUserByUserIdAsync(string userId, IEnumerable<string> roles);
        Task<IdentityResult> RemoveRolesFromUserByUserNameAsync(string userName, IEnumerable<string> roles);
        Task<bool> RoleExcistAsync(string role);
        Task<IdentityResult> UpdateAsync(string oldName, string newName);
    }
}