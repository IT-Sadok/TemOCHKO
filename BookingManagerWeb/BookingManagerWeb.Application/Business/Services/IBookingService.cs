using System.Security.Claims;
using BookingManagerWeb.Application.Business.DTOs;

namespace BookingManagerWeb.Application.Business.Services;

public interface IBookingService
{
    Task<BookingsCreateResponseDto> MakeBookingAsync(BookingCreateDto createDto, Claim userIdClaim, CancellationToken cancellationToken);
    Task<BookingsFetchResponseDto> FetchBookingsAsync(Claim userIdClaim, CancellationToken cancellationToken);
}