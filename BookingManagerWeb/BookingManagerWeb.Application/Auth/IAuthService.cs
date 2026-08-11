using BookingManagerWeb.Application.Auth.DTOs;

namespace BookingManagerWeb.Application.Auth;

public interface IAuthService
{
    Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto registerRequestDto, CancellationToken ct);
    //Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto, CancellationToken ct);
}