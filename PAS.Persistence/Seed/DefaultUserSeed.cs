using Microsoft.AspNetCore.Identity;

namespace Persistence.Seed
{
    public static class DefaultUserSeed
    {
        public static async Task SeedAsync(UserManager<IdentityUser> userManager)
        {
            var adminEmail = "admin@pms.com";

            var user = await userManager.FindByEmailAsync(adminEmail);

            if (user == null)
            {
                var admin = new IdentityUser
                {
                    UserName = "admin",
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(admin, "Admin@123");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }
        }
    }
}
