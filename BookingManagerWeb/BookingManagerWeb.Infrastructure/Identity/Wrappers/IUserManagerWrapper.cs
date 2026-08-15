using Microsoft.AspNetCore.Identity;

namespace BookingManagerWeb.Infrastructure.Identity.Wrappers;

public interface IUserManagerWrapper
{
    Task<IdentityResult> CreateAsync(ApplicationUser user, string password, CancellationToken cancellationToken = default);
    Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role, CancellationToken cancellationToken = default);
    Task<ApplicationUser?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = default);
    Task<bool> CheckPasswordAsync(ApplicationUser user, string password, CancellationToken cancellationToken = default);
}