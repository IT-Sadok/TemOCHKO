using Microsoft.AspNetCore.Identity;

namespace BookingManagerWeb.Infrastructure.Identity.Wrappers;

public class UserManagerWrapper (UserManager<ApplicationUser> userManager) : IUserManagerWrapper
{
    public async Task<IdentityResult> CreateAsync(ApplicationUser user, string password, CancellationToken cancellationToken = default)
    {
        return await userManager.CreateAsync(user, password);
    }

    public async Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role, CancellationToken cancellationToken = default)
    {
        return await userManager.AddToRoleAsync(user, role);
    }

    public async Task<ApplicationUser?> FindByEmailAsync(string normalizedEmail,
        CancellationToken cancellationToken = default)
    {
        return await userManager.FindByEmailAsync(normalizedEmail);
    }

    public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password, CancellationToken cancellationToken = default)
    {
        return await userManager.CheckPasswordAsync(user, password);
    }
}