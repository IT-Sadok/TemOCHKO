using BookingManagerWeb.Infrastructure.Identity;

namespace BookingManagerWeb.Application.Auth.Services;

public interface IJwtService
{
    string GenerateToken(ApplicationUser user);
}