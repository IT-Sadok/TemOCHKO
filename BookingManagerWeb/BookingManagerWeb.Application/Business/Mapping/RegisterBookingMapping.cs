using BookingManagerWeb.Application.Business.DTO_s;
using BookingManagerWeb.Domain.Models;
using Mapster;

namespace BookingManagerWeb.Application.Business.Mapping;

public class RegisterBookingMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Booking, BookingCreateDto>()
            .Map(dest => dest.ApartmentId, src => src.Apartment!.Id)
            .Map(dest => dest.StartDate, src => src.From)
            .Map(dest => dest.EndDate, src => src.To);
    }
}