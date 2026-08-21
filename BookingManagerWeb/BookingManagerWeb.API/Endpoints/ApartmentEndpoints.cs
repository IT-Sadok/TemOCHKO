using BookingManagerWeb.Application.Auth.DTOs;
using BookingManagerWeb.Application.Business.DTO_s;
using BookingManagerWeb.Application.Business.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BookingManagerWeb.Endpoints;

public static class ApartmentEndpoints
{
    public static void MapApartmentEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api");
        
        group.MapGet("/apartments", MapGetApartmentsAsync)
            .WithName("Apartments")
            .Produces<ApartmentsResponseDto>(StatusCodes.Status200OK)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound);
        
    }

    private static async Task<Results<Ok<ApartmentsResponseDto>, ValidationProblem, NotFound<ProblemDetails>>>
        MapGetApartmentsAsync([FromQuery] ApartmentSearchDto searchDto, 
            IApartmentService apartmentService, 
            CancellationToken cancellationToken)
    {
        var apartmentsSearchResult = await apartmentService.GetApartmentsAsync(searchDto, cancellationToken);
        return TypedResults.Ok(apartmentsSearchResult);
    }
    
}