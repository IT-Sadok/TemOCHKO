using BookingManagerWeb.Domain.Constants;

namespace BookingManagerWeb.Application.Business.DTO_s;

public class BookingsResponseDto
{
    public Guid Id { get; set; }
    public Guid ApartmentId { get; set; }
    public decimal TotalPrice { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public string Status { get; set; } = string.Empty;
}