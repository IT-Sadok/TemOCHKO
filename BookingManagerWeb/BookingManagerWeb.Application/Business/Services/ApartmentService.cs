using BookingManagerWeb.Application.Auth.DTOs;
using BookingManagerWeb.Application.Business.DTO_s;
using BookingManagerWeb.Infrastructure.Persistence;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace BookingManagerWeb.Application.Business.Services;

public class ApartmentService(ApplicationDbContext dbContext, IMapper mapper) : IApartmentService
{
    public async Task<ApartmentsResponseDto> GetApartmentsAsync(ApartmentSearchDto searchDto, CancellationToken cancellationToken)
    {
        var apartmentsSearchQuery = dbContext.Apartments.AsQueryable();

        if (searchDto.Guests.HasValue)
        {
            apartmentsSearchQuery = apartmentsSearchQuery.Where(a => searchDto.Guests <= a.MaxGuests);
        }

        if (searchDto.CheckIn.HasValue && searchDto.CheckOut.HasValue)
        {
            apartmentsSearchQuery = apartmentsSearchQuery.Where(a => !a.Bookings.Any(b => b.From < searchDto.CheckOut && b.To > searchDto.CheckIn));
        }

        var response = new ApartmentsResponseDto()
        { 
            Apartments = await apartmentsSearchQuery.ToListAsync(cancellationToken: cancellationToken)
        };
        
        return response;
    }
}