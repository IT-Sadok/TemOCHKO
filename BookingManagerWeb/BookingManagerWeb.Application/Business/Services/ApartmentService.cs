using BookingManagerWeb.Application.Business.DTOs;
using BookingManagerWeb.Application.Business.DTOs.Pagination;
using BookingManagerWeb.Application.Business.Extensions;
using BookingManagerWeb.Infrastructure.Persistence;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace BookingManagerWeb.Application.Business.Services;

public class ApartmentService(ApplicationDbContext dbContext, IMapper mapper) : IApartmentService
{
    public async Task<PagedResponse<ApartmentsFetchResponseDto>> GetApartmentsAsync(ApartmentQueryFilter filter, ApartmentSearchDto searchDto, CancellationToken cancellationToken)
    {
        var pageNumber = Math.Max(1, filter.PageNumber);
        var pageSize = Math.Clamp(filter.PageSize, 1, 50);

        var apartmentsSearchQuery = dbContext.Apartments.AsNoTracking().AsQueryable();
        
        if (searchDto.Guests.HasValue)
        {
            apartmentsSearchQuery = apartmentsSearchQuery.Where(a => searchDto.Guests <= a.MaxGuests);
        }

        if (searchDto.CheckIn.HasValue && searchDto.CheckOut.HasValue)
        {
            apartmentsSearchQuery = apartmentsSearchQuery.Where(a => !a.Bookings.Any(b => b.From < searchDto.CheckOut && b.To > searchDto.CheckIn));
        }
        
        var totalRecords = await apartmentsSearchQuery.CountAsync(cancellationToken);
        
        var response = await apartmentsSearchQuery
            .ApplyPagination(filter.PageNumber, filter.PageSize)
            .Select(a => new ApartmentsFetchResponseDto
            {
                Id = a.Id,
                Name = a.Name,
                PricePerNight = a.PricePerNight,
                MaxGuests = a.MaxGuests,
            })
            .ToListAsync(cancellationToken);
        
        return new PagedResponse<ApartmentsFetchResponseDto>()
        {
            Data = response, 
            PageNumber = pageNumber, 
            PageSize = pageSize,
            TotalRecords = totalRecords,
            TotalPages = (int)Math.Ceiling(totalRecords / (decimal)filter.PageSize) 
        };
    }
}