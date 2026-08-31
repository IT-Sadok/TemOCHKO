using System.Security.Claims;
using BookingManagerWeb.Application.Business.DTO_s;
using BookingManagerWeb.Domain.Constants;
using BookingManagerWeb.Domain.Models;
using BookingManagerWeb.Infrastructure.Persistence;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace BookingManagerWeb.Application.Business.Services;

public class BookingService(ApplicationDbContext dbContext, IMapper mapper) : IBookingService
{
    public async Task<BookingsCreateResponseDto> MakeBookingAsync(BookingCreateDto createDto, Claim subclaim, CancellationToken cancellationToken)
    {
        if (subclaim is null)
        {
            throw new ArgumentNullException(nameof(subclaim));
        }

        var apartment = await dbContext.Apartments.Include(apartment => apartment.Bookings).FirstOrDefaultAsync(x => x.Id == createDto.ApartmentId, cancellationToken: cancellationToken);
        if (apartment is null)
        {
            throw new ApartmentNotFoundException(nameof(apartment));
        }

        if (apartment.Bookings.Any(b => b.From < createDto.EndDate && b.To > createDto.StartDate))
        {
            throw new ApartmentOccupiedException(nameof(apartment));   
        }

        var totalPrice = (createDto.EndDate.DayNumber - createDto.StartDate.DayNumber) * apartment.PricePerNight;

        var booking = new Booking()
        {
            ApartmentId = apartment.Id,
            UserId = subclaim.Value,
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

    public async Task<BookingsFetchResponseDto> FetchBookingsAsync(Claim subClaim, CancellationToken cancellationToken)
    {
        if (subClaim is null)
        {
            throw new ArgumentNullException(nameof(subClaim));
        }

        var userBookings = await dbContext.Bookings.Where(b => b.UserId == subClaim.Value)
            .ToListAsync(cancellationToken: cancellationToken);

        return new BookingsFetchResponseDto()
        {
            Bookings = userBookings
        };
    }
}