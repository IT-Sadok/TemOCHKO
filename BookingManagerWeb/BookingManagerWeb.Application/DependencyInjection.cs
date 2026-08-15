using System.Reflection.Metadata;
using BookingManagerWeb.Application.Auth;
using BookingManagerWeb.Application.Auth.DTOs;
using BookingManagerWeb.Application.Auth.Mapping;
using BookingManagerWeb.Application.Auth.Services;
using BookingManagerWeb.Application.Auth.Validators;
using BookingManagerWeb.Infrastructure.Identity;
using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;

namespace BookingManagerWeb.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // services.AddValidatorsFromAssemblyContaining<AssemblyReference>();
        services.AddValidatorsFromAssemblyContaining<RegisterAuthMapping>();

        var config = TypeAdapterConfig.GlobalSettings;
        
        config.Scan(typeof(RegisterAuthMapping).Assembly);
        
        //config.Scan(typeof(AssemblyReference).Assembly);
        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();
        
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IValidator<RegisterRequestDto>, RegisterRequestDtoValidator>();
        services.AddScoped<IValidator<LoginRequestDto>, LoginRequestDtoValidator>();        
        return services;
    }
}