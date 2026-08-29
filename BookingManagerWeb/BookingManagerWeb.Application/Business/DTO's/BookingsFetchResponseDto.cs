using BookingManagerWeb.Domain.Models;

namespace BookingManagerWeb.Application.Business.DTO_s;

public sealed record BookingsFetchResponseDto
{
    public List<Booking> Bookings { get; init; } = new List<Booking>();
}