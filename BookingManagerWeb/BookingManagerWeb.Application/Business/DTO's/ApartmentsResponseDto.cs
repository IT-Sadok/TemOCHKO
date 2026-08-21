using BookingManagerWeb.Domain.Models;

namespace BookingManagerWeb.Application.Business.DTO_s;

public class ApartmentsResponseDto
{
    public List<Apartment> Apartments { get; init; } = new List<Apartment>();
}