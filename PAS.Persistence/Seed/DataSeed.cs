using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Seed
{
    public static class DataSeed
    {
        public static async Task SeedAsync(
            ApplicationDbContext context,
            RoleManager<IdentityRole> roleManager,
            UserManager<IdentityUser> userManager)
        {
            await context.Database.MigrateAsync();

            await CategorySeed.SeedAsync(context);

            await RoleSeed.SeedAsync(roleManager);

            await DefaultUserSeed.SeedAsync(userManager);
        }
    }
}
