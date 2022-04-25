using DaraSurvey.Entities;
using DaraSurvey.Extentions;
using DaraSurvey.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace DaraSurvey.Core
{
    public static class IdentityConfiguration
    {
        public static void IdentityConfig(this IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<DB>()
                .AddDefaultTokenProviders();
        }

        // --------------------

        public static async Task IdentitySeedAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            await SeedRoleAsync(roleManager);
            await SeedSuperAdminAsync(userManager);
        }

        // --------------------

        private static async Task SeedRoleAsync(RoleManager<IdentityRole> roleManager)
        {
            var roles = ExRole.GetStringRoles();

            foreach (var role in roles)
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole { Name = role });
        }

        // --------------------

        private static async Task SeedSuperAdminAsync(UserManager<User> userManager)
        {
            var rootUser = await userManager.FindByNameAsync("root");
            if (rootUser == null)
                await CreateSuperAdminUserAsync(userManager);
        }

        // --------------------

        private static async Task CreateSuperAdminUserAsync(UserManager<User> userManager)
        {
            var rootUser = new User
            {
                UserName = "root",
                Created = DateTime.UtcNow,
                FirstName = "root",
                LastName = "root",
                Email = "root@domain.com",
                PhoneNumber = "09128591861",
                CountryCode = 98,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
            };

            var identityResult = await userManager.CreateAsync(rootUser, "rooT@123456");
            if (identityResult.Succeeded)
                await userManager.AddToRoleAsync(rootUser, $"{Role.root}");
        }
    }
}
