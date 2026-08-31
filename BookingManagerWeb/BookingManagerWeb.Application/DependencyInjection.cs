using System.Reflection;
using BookingManagerWeb.Application.Auth.Mapping;
using BookingManagerWeb.Application.Auth.Services;
using BookingManagerWeb.Application.Business.Mapping;
using BookingManagerWeb.Application.Business.Services;
using BookingManagerWeb.Infrastructure.Auth;
using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;

namespace BookingManagerWeb.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<RegisterAuthMapping>();
        
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(typeof(RegisterAuthMapping).Assembly);
        TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());        
        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();
        
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IApartmentService, ApartmentService>();
        services.AddScoped<IBookingService, BookingService>();
        return services;
    }
}