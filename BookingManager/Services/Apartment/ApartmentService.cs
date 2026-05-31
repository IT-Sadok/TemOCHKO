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
    
    public List<ApartmentListItemDTO> GetApartmentsOfHost(int hostId)
    {
        var apartList = new List<ApartmentListItemDTO>();
        foreach (var apartmentDb in _apartmentRepository.GetApartmentsOfHost(hostId))
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

    public void SaveApartments()
    {
        _apartmentRepository.SaveApartments();
    }
}