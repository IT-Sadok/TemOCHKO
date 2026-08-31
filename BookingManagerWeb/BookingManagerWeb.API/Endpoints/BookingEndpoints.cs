using System.Security.Claims;
using BookingManagerWeb.Application.Business.DTOs;
using BookingManagerWeb.Application.Business.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BookingManagerWeb.Endpoints;

public static class BookingEndpoints
{
    public static void MapBookingsEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/bookings")
            .WithName("Bookings");
        
        group.MapPost("/", PostBooking)
            .WithName("PostBooking")
            .Produces<BookingsCreateResponseDto>(StatusCodes.Status200OK)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequireAuthorization();

        group.MapGet("/", GetBookings)
            .WithName("GetBookings")
            .Produces<BookingsFetchResponseDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequireAuthorization();

    }

    private static async Task<Results<Ok<BookingsFetchResponseDto>, NotFound<ProblemDetails>>> 
        GetBookings(
            ClaimsPrincipal user, 
            IBookingService bookingService,
            CancellationToken cancellationToken)
    {
        var userIdClaim = user.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);
        var response = await bookingService.FetchBookingsAsync(userIdClaim, cancellationToken);
        return TypedResults.Ok(response);
    }

    private static async Task<Results<Ok<BookingsCreateResponseDto>, ValidationProblem, NotFound<ProblemDetails>>>
        PostBooking(
            BookingCreateDto createDto, 
            ClaimsPrincipal user,
            IBookingService bookingService, 
            CancellationToken cancellationToken)

    {
        var userIdClaim = user.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);
        var response = await bookingService.MakeBookingAsync(createDto, userIdClaim, cancellationToken);
        return TypedResults.Ok(response);
    }
}