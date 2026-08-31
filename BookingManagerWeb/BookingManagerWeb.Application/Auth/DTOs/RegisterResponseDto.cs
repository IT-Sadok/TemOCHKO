namespace BookingManagerWeb.Application.Auth.DTOs;

public sealed record RegisterResponseDto
{
    public string Id { get; init; }
    public string AccessToken { get; init; }
}