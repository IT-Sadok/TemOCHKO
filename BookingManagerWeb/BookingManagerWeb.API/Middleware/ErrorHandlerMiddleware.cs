using System.Text.Json;
using BookingManagerWeb.Application.Auth;
using Microsoft.AspNetCore.Mvc;
using Scalar.AspNetCore.Attributes;

namespace BookingManagerWeb.Middleware;

[Deprecated]
public class ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (AuthException ex)
        {
            logger.LogError(ex.Message);
            
            var response = context.Response;
            response.StatusCode = StatusCodes.Status400BadRequest;
            var problemDetails = new ProblemDetails()
            {
                Status = response.StatusCode,
                Title = "Authentication error",
                Detail = ex.Message
            };
            var result = JsonSerializer.Serialize(problemDetails);
            await response.WriteAsync(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            
            var response = context.Response;
            response.StatusCode = StatusCodes.Status500InternalServerError;
            var problemDetails = new ProblemDetails()
            {
                Status = response.StatusCode,
                Title = "Internal Server Error",
                Detail = ex.Message
            };
            var result = JsonSerializer.Serialize(problemDetails);
            await response.WriteAsync(result);
        }
    }
}