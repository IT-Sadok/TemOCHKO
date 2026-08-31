namespace BookingManagerWeb.Domain.Models;

public class Booking
{
    public Guid Id { get; set; }
    public Guid ApartmentId { get; set; } 
    public string UserId { get; set; } = string.Empty;
    public DateOnly From { get; set; }
    public DateOnly To { get; set; }
    public decimal TotalPrice { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}