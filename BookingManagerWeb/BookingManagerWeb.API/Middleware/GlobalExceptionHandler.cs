using BookingManagerWeb.Application.Auth;
using BookingManagerWeb.Application.Business;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BookingManagerWeb.Middleware;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var problemDetails = new ProblemDetails();
        problemDetails.Instance = httpContext.Request.Path;
        if (exception is AuthException authException)
        {
            problemDetails.Status = StatusCodes.Status400BadRequest;
            problemDetails.Title = "Authentication error";
            problemDetails.Detail = authException.Message;
        }
        else if (exception is ApartmentOccupiedException apartmentOccupiedException)
        {
            problemDetails.Status = StatusCodes.Status400BadRequest;
            problemDetails.Title = "Apartment already occupied for these dates";
            problemDetails.Detail = apartmentOccupiedException.Message;
        }
        else if (exception is ApartmentNotFoundException apartmentNotFoundException)
        {
            problemDetails.Status = StatusCodes.Status404NotFound;
            problemDetails.Title = "Apartment not found";
            problemDetails.Detail = apartmentNotFoundException.Message;
        }
        else
        {
            problemDetails.Status = StatusCodes.Status500InternalServerError;
            problemDetails.Title = "Internal server error";
            problemDetails.Detail = exception.Message;
        }
        logger.LogError("{ProblemDetailsTitle}", problemDetails.Title);
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken: cancellationToken);
        return true;
    }
}