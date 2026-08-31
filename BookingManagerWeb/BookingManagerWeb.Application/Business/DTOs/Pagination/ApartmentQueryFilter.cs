namespace BookingManagerWeb.Application.Business.DTOs.Pagination;

public class ApartmentQueryFilter
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 5;
}