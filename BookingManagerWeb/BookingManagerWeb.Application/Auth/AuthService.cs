using BookingManagerWeb.Application.Auth.DTOs;

namespace BookingManagerWeb.Application.Auth;

public class AuthService : IAuthService
{
    public Task<RegisterResponseDto> Register(RegisterRequestDto registerRequestDto, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}