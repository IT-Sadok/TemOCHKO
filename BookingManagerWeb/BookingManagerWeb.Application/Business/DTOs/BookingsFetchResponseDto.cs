using BookingManagerWeb.Domain.Models;

namespace BookingManagerWeb.Application.Business.DTOs;

public sealed record BookingsFetchResponseDto
{
    public List<Booking> Bookings { get; init; } = new List<Booking>();
}