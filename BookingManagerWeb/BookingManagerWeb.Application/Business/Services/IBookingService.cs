using System.Security.Claims;
using BookingManagerWeb.Application.Business.DTO_s;

namespace BookingManagerWeb.Application.Business.Services;

public interface IBookingService
{
    Task<BookingsCreateResponseDto> MakeBookingAsync(BookingCreateDto createDto, Claim subClaim, CancellationToken cancellationToken);
    Task<BookingsFetchResponseDto> FetchBookingsAsync(Claim subClaim, CancellationToken cancellationToken);
}