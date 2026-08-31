using BookingManagerWeb.Domain.Models;

namespace BookingManagerWeb.Application.Business.DTOs;

public class ApartmentsFetchResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal PricePerNight { get; set; }
    public int MaxGuests { get; set; }
}