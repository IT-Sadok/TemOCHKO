using BookingManagerWeb.Application.Business.DTOs;
using BookingManagerWeb.Application.Business.DTOs.Pagination;
using BookingManagerWeb.Application.Business.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BookingManagerWeb.Endpoints;

public static class ApartmentEndpoints
{
    public static void MapApartmentEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/apartments");
        
        group.MapGet("/", GetApartmentsAsync)
            .WithName("Apartments")
            .Produces<ApartmentsFetchResponseDto>(StatusCodes.Status200OK)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound);
        
    }

    private static async Task<Results<Ok<PagedResponse<ApartmentsFetchResponseDto>>, ValidationProblem, NotFound<ProblemDetails>>>
        GetApartmentsAsync([AsParameters] ApartmentQueryFilter filter,
            [AsParameters] ApartmentSearchDto searchDto, 
            IApartmentService apartmentService, 
            CancellationToken cancellationToken)
    {
        var apartmentsSearchResult = await apartmentService.GetApartmentsAsync(filter, searchDto, cancellationToken);
        return TypedResults.Ok(apartmentsSearchResult);
    }
    
}