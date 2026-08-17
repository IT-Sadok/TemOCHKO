using A2v10.Identity.Jwt;
using BookingManagerWeb.Infrastructure.Identity;

namespace BookingManagerWeb.Infrastructure.Auth;

public interface IJwtService
{
    string GenerateToken(ApplicationUser user);
}