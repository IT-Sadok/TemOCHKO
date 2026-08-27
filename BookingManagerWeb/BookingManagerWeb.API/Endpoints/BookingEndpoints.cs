using System.Security.Claims;
using BookingManagerWeb.Application.Business.DTO_s;
using BookingManagerWeb.Application.Business.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BookingManagerWeb.Endpoints;

public static class BookingEndpoints
{
    public static void MapBookingsEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api")
            .WithName("Bookings");
        
        group.MapPost("/bookings/", PostBooking)
            .WithName("GetBookingById")
            .Produces<BookingsResponseDto>(StatusCodes.Status200OK)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequireAuthorization();
    }

    private static async Task<Results<Ok<BookingsResponseDto>, ValidationProblem, NotFound<ProblemDetails>>>
        PostBooking(
            BookingCreateDto createDto, 
            ClaimsPrincipal user,
            IBookingService bookingService, 
            CancellationToken cancellationToken)

    {
        var sub = user.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);
        var response = await bookingService.MakeBookingAsync(createDto, sub, cancellationToken);
        return TypedResults.Ok(response);
    }
}