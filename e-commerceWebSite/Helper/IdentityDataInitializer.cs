using Castle.Core.Internal;
using e_commerceWebSite.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace e_commerceWebSite.Helper
{
    public static class IdentityDataInitializer
    {
        public static async Task SeedRolesAndUsersAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            string[] roleNames = { "Admin", "Seller", "User" };

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    var role = new IdentityRole { Name = roleName };
                    await roleManager.CreateAsync(role);
                }
            }

            ApplicationUser adminUser = new ApplicationUser
            {
                UserName = "Admin_Ahmed",
                Email = "admin@gmail.com",
                IsActive = true,
                PasswordHash = "AdminPassword123",

            };
            IEnumerable<ApplicationUser> Admins = await userManager.GetUsersInRoleAsync("Admin");
            if (Admins.IsNullOrEmpty())
            {
                IdentityResult result = await userManager.CreateAsync(adminUser, adminUser.PasswordHash);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");

                }
            }
        }
    }
}
