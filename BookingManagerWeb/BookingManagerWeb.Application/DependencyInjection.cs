using System.Reflection.Metadata;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace BookingManagerWeb.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<AssemblyReference>();
        return services;
    }
}