using BookingManagerWeb.Application.Business.DTOs;
using BookingManagerWeb.Application.Business.DTOs.Pagination;

namespace BookingManagerWeb.Application.Business.Services;

public interface IApartmentService
{
    Task<PagedResponse<ApartmentsFetchResponseDto>> GetApartmentsAsync(ApartmentQueryFilter filter, ApartmentSearchDto searchDto, CancellationToken cancellationToken);
}