using Models.DTOs;
using Repositories;

namespace Services;

public class ApartmentService : IApartmentService
{
    private IApartmentRepository _apartmentRepository;
    public ApartmentService(IApartmentRepository apartmentRepository)
    {
        _apartmentRepository = apartmentRepository;
    }
    
    public List<ApartmentListDTO> GetApartmentsOfHost(int hostId)
    {
        var apartList = new List<ApartmentListDTO>();
        foreach (var apartmentDb in _apartmentRepository.GetApartmentsOfHost(hostId))
            apartList.Add(new ApartmentListDTO(apartmentDb));
        return apartList;
    }
}