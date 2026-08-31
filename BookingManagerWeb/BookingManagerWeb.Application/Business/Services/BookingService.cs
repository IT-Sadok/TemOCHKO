using System.Security.Claims;
using BookingManagerWeb.Application.Business.DTOs;
using BookingManagerWeb.Domain.Constants;
using BookingManagerWeb.Domain.Models;
using BookingManagerWeb.Infrastructure.Persistence;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace BookingManagerWeb.Application.Business.Services;

public class BookingService(ApplicationDbContext dbContext, IMapper mapper) : IBookingService
{
    public async Task<BookingsCreateResponseDto> MakeBookingAsync(BookingCreateDto createDto, Claim userIdClaim, CancellationToken cancellationToken)
    {
        if (userIdClaim is null)
        {
            throw new ArgumentNullException(nameof(userIdClaim));
        }

        var apartment = await dbContext.Apartments.FirstOrDefaultAsync(x => x.Id == createDto.ApartmentId, cancellationToken: cancellationToken);
        if (apartment is null)
        {
            throw new ApartmentNotFoundException(nameof(apartment));
        }
        
        if (dbContext.Bookings.Any(b => b.ApartmentId == apartment.Id && (b.From < createDto.EndDate && b.To > createDto.StartDate)))
        {
            throw new ApartmentOccupiedException(nameof(apartment));   
        }

        var totalPrice = (createDto.EndDate.DayNumber - createDto.StartDate.DayNumber) * apartment.PricePerNight;

        var booking = new Booking()
        {
            ApartmentId = apartment.Id,
            UserId = userIdClaim.Value,
            From = createDto.StartDate,
            To = createDto.EndDate,
            TotalPrice = totalPrice,
            Status = BookingStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };
        
        dbContext.Bookings.Add(booking);
        await dbContext.SaveChangesAsync(cancellationToken);

        var response = mapper.Map<BookingsCreateResponseDto>(booking);
        return response;
    }

    public async Task<BookingsFetchResponseDto> FetchBookingsAsync(Claim userIdClaim, CancellationToken cancellationToken)
    {
        if (userIdClaim is null)
        {
            throw new ArgumentNullException(nameof(userIdClaim));
        }

        var userBookings = await dbContext.Bookings.Where(b => b.UserId == userIdClaim.Value)
            .ToListAsync(cancellationToken: cancellationToken);

        return new BookingsFetchResponseDto()
        {
            Bookings = userBookings
        };
    }
}