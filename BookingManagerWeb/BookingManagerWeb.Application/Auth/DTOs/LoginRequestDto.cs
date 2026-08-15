namespace BookingManagerWeb.Application.Auth.DTOs;

public sealed record LoginRequestDto
{
    public string Email { get; init; } 
    public string Password { get; init; }
}