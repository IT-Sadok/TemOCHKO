using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BookingManagerWeb.Infrastructure.Identity;

public static class IdentityDataSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var logger =  scope.ServiceProvider
            .GetRequiredService<ILoggerFactory>()
            .CreateLogger(typeof(UserManager<IdentityUser>));

        foreach (var role in roleManager.Roles)
        {
            if (await roleManager.RoleExistsAsync(role.Name))
            {
                continue;
            }
            
            var result = await roleManager.CreateAsync(new IdentityRole(role.Name));
            if (!result.Succeeded)
            {
                logger.LogError($"Failed to create role {role.Name}");
            }
        }
    }
    
}