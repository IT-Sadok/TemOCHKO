namespace BookingManagerWeb.Infrastructure.Identity.Wrappers;

public interface IRoleManagerWrapper
{
    Task<bool> RoleExistsAsync(string role, CancellationToken cancellationToken = default);
}