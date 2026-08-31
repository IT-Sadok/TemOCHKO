namespace BookingManagerWeb.Application.Business.DTOs;

public sealed record ApartmentSearchDto
{
    public DateOnly? CheckIn { get; init; }
    public DateOnly? CheckOut { get; init; }
    public int? Guests { get; init; }
}