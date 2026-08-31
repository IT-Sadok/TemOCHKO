using BookingManagerWeb.Domain.Models;

namespace BookingManagerWeb.Application.Business.DTOs;

public class ApartmentsResponseDto
{
    public List<Apartment> Apartments { get; init; } = new List<Apartment>();
}