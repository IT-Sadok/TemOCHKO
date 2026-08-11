using BookingManagerWeb.Middleware;

namespace BookingManagerWeb.Extensions;

public static class ErrorHandingExtensions
{
    public static void AddExceptionHandlers(this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
    }
}