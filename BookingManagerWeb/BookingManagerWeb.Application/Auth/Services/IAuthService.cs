using BookingManagerWeb.Application.Auth.DTOs;

namespace BookingManagerWeb.Application.Auth.Services;

public interface IAuthService
{
    Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto registerRequestDto, CancellationToken ct);
    Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequestDto, CancellationToken ct);
}