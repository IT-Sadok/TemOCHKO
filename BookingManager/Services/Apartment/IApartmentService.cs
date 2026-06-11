using Models.DTOs;

namespace Services.Apartment;

public interface IApartmentService
{
    Task<List<ApartmentListItemDTO>> GetApartmentsOfHostAsync(int hostId);
    Task<List<ApartmentListItemDTO>> GetAllApartmentsAsync();
    Task SaveApartmentsAsync();
}