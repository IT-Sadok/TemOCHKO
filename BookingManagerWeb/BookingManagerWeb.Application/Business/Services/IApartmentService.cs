using BookingManagerWeb.Application.Business.DTO_s;

namespace BookingManagerWeb.Application.Business.Services;

public interface IApartmentService
{
    Task<ApartmentsResponseDto> GetApartmentsAsync(ApartmentSearchDto searchDto, CancellationToken cancellationToken);
}