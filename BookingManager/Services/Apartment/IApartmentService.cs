using Models.DTOs;

namespace Services.Apartment;

public interface IApartmentService
{
    List<ApartmentListItemDTO> GetApartmentsOfHost(int hostId);
    void SaveApartments();
}