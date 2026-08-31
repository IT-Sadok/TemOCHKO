using BookingManagerWeb.Application.Business.DTOs;
using BookingManagerWeb.Domain.Models;
using Mapster;

namespace BookingManagerWeb.Application.Business.Mapping;

public class RegisterBookingMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Booking, BookingsCreateResponseDto>()
            .Map(dest => dest.ApartmentId, src => src.ApartmentId)
            .Map(dest => dest.StartDate, src => src.From)
            .Map(dest => dest.EndDate, src => src.To);
    }
}