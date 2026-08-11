using BookingManagerWeb.Application.Auth;
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