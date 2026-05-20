using BookingManager.DBModels;
using BookingManager.MainApp;

namespace BookingManager.Services;

public class ApartmentService
{
    private StorageContext _context;

    public ApartmentService(StorageContext context)
    {
        _context = context;
    }

    public List<ApartmentDTO> GetApartmentsOfHost(int hostId)
    {
        var hostApartments = new List<ApartmentDTO>();
        foreach (var apartment in _context.GetAllApartments())
        {
            if (apartment.HostId == hostId)
            {
                hostApartments.Add(new ApartmentDTO(apartment));
            }
        }
        return hostApartments;
    }
}