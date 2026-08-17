namespace BookingManagerWeb.Application.Auth.DTOs;

public sealed record RegisterRequestDto
{
    public string Email { get; init; }
    public string Password { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Role { get; set; }
}