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
    public async Task<BookingsResponseDto> MakeBookingAsync(BookingCreateDto createDto, Claim subclaim, CancellationToken cancellationToken)
    {
        if (subclaim is null)
        {
            throw new ArgumentNullException(nameof(subclaim));
        }

        var apartment = await dbContext.Apartments.FirstOrDefaultAsync(x => x.Id == createDto.ApartmentId, cancellationToken: cancellationToken);
        if (apartment is null)
        {
            throw new ApartmentNotFoundException(nameof(apartment));
        }

        var totalPrice = (createDto.EndDate.DayNumber - createDto.StartDate.DayNumber) * apartment.PricePerNight;

        var booking = new Booking()
        {
            Apartment = apartment,
            UserId = subclaim.Value,
            From = createDto.StartDate,
            To = createDto.EndDate,
            TotalPrice = totalPrice,
            Status = BookingStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };
        
        dbContext.Bookings.Add(booking);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        // use mapping
        return new BookingsResponseDto()
        {
            Id =  booking.Id,
            ApartmentId = booking.Apartment.Id,
            StartDate = booking.From, 
            EndDate = booking.To,
            TotalPrice = booking.TotalPrice,
            Status = booking.Status.ToString(),
        };
    }
}