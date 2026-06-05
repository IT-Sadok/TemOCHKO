using Models.DTOs;
using Repositories.Apartment;

namespace Services.Apartment;

public class ApartmentService : IApartmentService
{
    private IApartmentRepository _apartmentRepository;
    public ApartmentService(IApartmentRepository apartmentRepository)
    {
        _apartmentRepository = apartmentRepository;
    }
    
    public async Task<List<ApartmentListItemDTO>> GetApartmentsOfHostAsync(int hostId)
    {
        var apartList = new List<ApartmentListItemDTO>();
        var hostDbApartments = await _apartmentRepository.GetApartmentsOfHostAsync(hostId);
        
        foreach (var apartmentDb in hostDbApartments)
        {
            apartList.Add(new ApartmentListItemDTO
            {
                Id = apartmentDb.Id,
                HostId = apartmentDb.HostId,
                Name = apartmentDb.Name,
                Type = apartmentDb.Type,
                PricePerNight = apartmentDb.PricePerNight,
                Rating = apartmentDb.Rating,
            });
        }
        return apartList;
    }

    public Task SaveApartmentsAsync()
    {
        return _apartmentRepository.SaveApartmentsAsync();
    }
}