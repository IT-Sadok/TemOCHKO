using Microsoft.AspNetCore.Identity;

namespace BookingManagerWeb.Infrastructure.Identity.Wrappers;

public interface IUserManagerWrapper
{
    public Task<IdentityResult> CreateAsync(ApplicationUser user, string password, CancellationToken cancellationToken = default);
    public Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role, CancellationToken cancellationToken = default);
    public Task<ApplicationUser?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = default);
    public Task<bool> CheckPasswordAsync(ApplicationUser user, string password, CancellationToken cancellationToken = default);
}