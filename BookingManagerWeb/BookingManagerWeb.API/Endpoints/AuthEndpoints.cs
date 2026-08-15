using BookingManagerWeb.Application.Auth.DTOs;
using BookingManagerWeb.Application.Auth.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
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
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status400BadRequest);
        
        group.MapPost("/login", MapLoginAsync)
            .WithName("Login")
            .Produces<LoginResponseDto>()
            .ProducesValidationProblem()
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
        
        var authServiceResponse = await authService.RegisterAsync(model, cancellationToken);
        return TypedResults.Created($"/auth/register/{authServiceResponse.Id}", authServiceResponse);
    }

    private static async Task<Results<Ok<LoginResponseDto>, ValidationProblem, BadRequest<ProblemDetails>>> MapLoginAsync(
            LoginRequestDto model,
            IAuthService authService,
            IValidator<LoginRequestDto> validator,
            CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(model, cancellationToken);
        if (!validationResult.IsValid)
        {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        var authLoginResponse = await authService.LoginAsync(model, cancellationToken);
        return TypedResults.Ok(authLoginResponse);
    }
}