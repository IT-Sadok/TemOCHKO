namespace BookingManagerWeb.Domain.Models;

public class Apartment
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal PricePerNight { get; set; }
    public int MaxGuests { get; set; }
    public bool IsActive { get; set; }
    public List<Booking> Bookings { get; set; } = new List<Booking>();
}