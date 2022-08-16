using HRSystem.Constants;
using HRSystem.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace HRSystem.Seeds
{
    public static class DefultUseres
    {
        public static async Task seedUser(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var defultUser = new ApplicationUser
            {
                Name = "Ali Ahmed Mosa",
                Email = "admin@admin.com",
                UserName = "admin@admin.com"
            };
            var user = await userManager.FindByEmailAsync(defultUser.Email);
            if(user == null)
            {
                await userManager.CreateAsync(defultUser, "AdminPass@admin123");
                await userManager.AddToRoleAsync(defultUser, "Admin");
            }
            //TODO: seed Claims
            await roleManager.seedClaimsForAdmin();
        }
        public static async Task seedClaimsForAdmin(this RoleManager<IdentityRole> roleManager)
        {
            var adminRole = await roleManager.FindByNameAsync("Admin");
            await roleManager.addPermissionsclaims(adminRole, "Users");
            await roleManager.addPermissionsclaims(adminRole, "Attendance");
            await roleManager.addPermissionsclaims(adminRole, "Employee");
            await roleManager.addPermissionsclaims(adminRole, "GeneralSettings");
            await roleManager.addPermissionsclaims(adminRole, "Holidays");
            await roleManager.addPermissionsclaims(adminRole, "Home");
            await roleManager.addPermissionsclaims(adminRole, "Roles");
            await roleManager.addPermissionsclaims(adminRole, "SalaryReport");
            await roleManager.addPermissionsclaims(adminRole, "Users");

        }
        public static async Task addPermissionsclaims(this RoleManager<IdentityRole> roleManager , IdentityRole role , string module) 
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            var allPermissions = Permissions.generatePermissionsList(module);
            foreach (var permission in allPermissions)
            {
                if(!allClaims.Any(c=>c.Type=="Permission" && c.Value == permission))
                {
                    await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
                }
            }
        } 

    }
}
