using Models;
using Models.DTOs;

namespace Services;

public interface IApartmentService
{
    List<ApartmentListDTO> GetApartmentsOfHost(int hostId);
    void SaveApartments();
}