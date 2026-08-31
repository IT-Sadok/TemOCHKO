namespace BookingManagerWeb.Application.Business.DTOs;

public class BookingsCreateResponseDto
{
    public Guid Id { get; set; }
    public Guid ApartmentId { get; set; }
    public decimal TotalPrice { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public string Status { get; set; } = string.Empty;
}