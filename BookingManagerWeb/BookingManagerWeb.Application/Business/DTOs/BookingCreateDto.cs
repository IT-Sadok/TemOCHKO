namespace BookingManagerWeb.Application.Business.DTOs;

public sealed record BookingCreateDto
{
    public Guid ApartmentId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
}