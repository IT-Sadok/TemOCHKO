using BookingManagerWeb.Application.Auth;
using BookingManagerWeb.Application.Auth.DTOs;
using BookingManagerWeb.Infrastructure.Identity;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace BookingManagerWeb.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthorization(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/auth");
        
        group.MapPost("/register", MapRegisterAsync)
            .WithName("Register")
            .Produces<RegisterResponseDto>(StatusCodes.Status201Created)
            .ProducesValidationProblem<>()
            .ProducesProblem(StatusCodes.Status400BadRequest);
    }

    private static async Task<Results<Created<RegisterResponseDto>, ValidationProblem, BadRequest<ProblemDetails>>> MapRegisterAsync(
        RegisterRequestDto model,
        IAuthService authService, 
        IValidator<RegisterRequestDto> validator,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(model);
        if (!validationResult.IsValid)
        {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        try
        {
            var authServiceResponse = await authService.RegisterAsync(model, cancellationToken);
            return TypedResults.Created($"/auth/register/{authServiceResponse.Id}", authServiceResponse);
        }
        catch (AuthException exception)
        {
            return TypedResults.BadRequest(new ProblemDetails
            {
                Title = "User Registration failed",
                Detail = exception.Message,
                Extensions = exception.Errors
            });
        }
    }
}