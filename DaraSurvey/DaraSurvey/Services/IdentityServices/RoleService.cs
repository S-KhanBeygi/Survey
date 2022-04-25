using DaraSurvey.Entities;
using DaraSurvey.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaraSurvey.Services
{
    public class RoleService : IRoleService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // --------------------

        public IEnumerable<IdentityRole> GetAll()
        {
            return _roleManager.Roles.Where(o => true);
        }

        // --------------------

        public async Task<IEnumerable<string>> GetUserRolesAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var userRoles = await _userManager.GetRolesAsync(user);

            return userRoles;
        }

        // --------------------

        public async Task<IdentityResult> CreateAsybc(string role)
        {
            return await _roleManager.CreateAsync(new IdentityRole { Name = role });
        }

        // --------------------

        public async Task<bool> RoleExcistAsync(string role)
        {
            return await _roleManager.RoleExistsAsync(role);
        }

        // --------------------

        public async Task<IdentityResult> DeleteAsync(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            return await _roleManager.DeleteAsync(role);
        }

        // --------------------

        public async Task<IdentityResult> UpdateAsync(string oldName, string newName)
        {
            var role = await _roleManager.FindByNameAsync(oldName);
            role.Name = newName;

            return await _roleManager.UpdateAsync(role);
        }

        // --------------------

        public async Task<IdentityResult> AddToRolesAsync(string userId, IEnumerable<string> roles)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return await _userManager.AddToRolesAsync(user, roles);
        }

        // --------------------

        public async Task<IdentityResult> AddRolesToUserAsync(string userName, IEnumerable<string> roles)
        {
            var user = await _userManager.FindByEmailAsync(userName);
            return await _userManager.AddToRolesAsync(user, roles);
        }

        // --------------------

        public async Task<IdentityResult> AddRoleToUserAsync(string userId, string roleId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var role = await _roleManager.FindByIdAsync(roleId);

            return await _userManager.AddToRoleAsync(user, role.Name);
        }

        // --------------------

        public async Task<IdentityResult> RemoveRolesFromUserByUserIdAsync(string userId, IEnumerable<string> roles)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return await _userManager.RemoveFromRolesAsync(user, roles);
        }

        // --------------------

        public async Task<IdentityResult> RemoveRolesFromUserByUserNameAsync(string userName, IEnumerable<string> roles)
        {
            var user = await _userManager.FindByNameAsync(userName);
            return await _userManager.RemoveFromRolesAsync(user, roles);
        }

        // --------------------

        public IEnumerable<string> GetUserIdsByRole(IEnumerable<string> roles)
        {
            IQueryable<IdentityRole> result = null;

            foreach (var role in roles)
                result = result.Union(_roleManager.Roles.Where(o => o.Name == role));

            return result.Select(o => o.Id);
        }
    }
}
