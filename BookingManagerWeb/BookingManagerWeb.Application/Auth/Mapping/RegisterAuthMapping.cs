using BookingManagerWeb.Application.Auth.DTOs;
using BookingManagerWeb.Infrastructure.Identity;
using Mapster;

namespace BookingManagerWeb.Application.Auth.Mapping;

public class RegisterAuthMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RegisterRequestDto, ApplicationUser>()
            .Map(dest => dest.UserName, src => src.Email)
            .Map(dest => dest.CreatedAt, _ => DateTime.UtcNow);

        config.NewConfig<ApplicationUser, RegisterResponseDto>()
            .Map(dest => dest.Id, src => src.Id);
    }
}