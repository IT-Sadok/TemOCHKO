using BookingManagerWeb.Application.Business.DTOs;

namespace BookingManagerWeb.Application.Business.Services;

public interface IApartmentService
{
    Task<ApartmentsResponseDto> GetApartmentsAsync(ApartmentSearchDto searchDto, CancellationToken cancellationToken);
}