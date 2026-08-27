namespace BookingManagerWeb.Application.Business.DTO_s;

public sealed record BookingCreateDto
{
    public Guid ApartmentId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
}