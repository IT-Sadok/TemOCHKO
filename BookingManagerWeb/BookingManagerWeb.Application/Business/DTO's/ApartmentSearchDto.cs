namespace BookingManagerWeb.Application.Business.DTO_s;

public sealed record ApartmentSearchDto
{
    public DateOnly? CheckIn { get; init; }
    public DateOnly? CheckOut { get; init; }
    public int? Guests { get; init; }
}