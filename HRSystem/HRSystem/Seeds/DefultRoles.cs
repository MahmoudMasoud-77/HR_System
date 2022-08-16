using Microsoft.AspNetCore.Identity;

namespace HRSystem.Seeds
{
    public static class DefultRoles
    {
        public static async Task seedAsync(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }
        } 
    }

}
