namespace BookingManagerWeb.Application.Business.DTO_s;

public sealed record ApartmentSearchDto
{
    public DateTime? CheckIn { get; init; }
    public DateTime? CheckOut { get; init; }
    public int? Guests { get; init; }
}