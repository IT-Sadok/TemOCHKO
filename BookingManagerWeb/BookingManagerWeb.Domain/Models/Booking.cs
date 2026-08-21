namespace BookingManagerWeb.Domain.Models;

public class Booking
{
    public Guid Id { get; set; }
    public Apartment? Apartment { get; set; } 
    public string UserId { get; set; } = string.Empty;
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public decimal TotalPrice { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}