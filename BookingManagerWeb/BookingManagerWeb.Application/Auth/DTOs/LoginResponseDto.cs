namespace BookingManagerWeb.Application.Auth.DTOs;

public sealed record LoginResponseDto
{
    public string Token { get; init; }
}