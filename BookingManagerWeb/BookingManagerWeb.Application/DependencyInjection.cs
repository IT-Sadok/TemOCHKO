using System.Reflection.Metadata;
using BookingManagerWeb.Application.Auth;
using BookingManagerWeb.Application.Auth.DTOs;
using BookingManagerWeb.Application.Auth.Validators;
using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;

namespace BookingManagerWeb.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<AssemblyReference>();

        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(typeof(AssemblyReference).Assembly);
        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();
        
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IValidator<RegisterRequestDto>, RegisterRequestDtoValidator>();
        
        return services;
    }
}