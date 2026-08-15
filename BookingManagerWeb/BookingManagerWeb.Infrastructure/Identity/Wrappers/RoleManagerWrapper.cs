using Microsoft.AspNetCore.Identity;

namespace BookingManagerWeb.Infrastructure.Identity.Wrappers;

public class RoleManagerWrapper(RoleManager<IdentityRole> roleManager) : IRoleManagerWrapper
{
    public async Task<bool> RoleExistsAsync(string role, CancellationToken cancellationToken = default)
    {
        return await roleManager.RoleExistsAsync(role);
    }
}