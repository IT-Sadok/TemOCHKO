using Microsoft.AspNetCore.Identity;

namespace BookingManagerWeb.Infrastructure.Identity.Wrappers;

public abstract class IUserManagerWrapper
{
    public abstract Task<IdentityResult> CreateAsync(ApplicationUser user, string password, CancellationToken cancellationToken = default);
    public abstract Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role, CancellationToken cancellationToken = default);
    public abstract Task<ApplicationUser?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = default);
    public abstract Task<bool> CheckPasswordAsync(ApplicationUser user, string password, CancellationToken cancellationToken = default);
}