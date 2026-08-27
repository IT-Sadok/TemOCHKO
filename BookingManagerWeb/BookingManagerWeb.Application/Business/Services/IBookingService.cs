using System.Security.Claims;
using BookingManagerWeb.Application.Business.DTO_s;

namespace BookingManagerWeb.Application.Business.Services;

public interface IBookingService
{
    Task<BookingsResponseDto> MakeBookingAsync(BookingCreateDto createDto, Claim subClaim, CancellationToken cancellationToken);
}