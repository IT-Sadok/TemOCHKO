using BookingManager.DBModels;

namespace BookingManager.Services;

public class StorageService
{
    private List<HostDBModel> _hostDbModelsList;
    private List<ApartmentDBModel> _apartmentDbModelsList;

    public void LoadData()
    {
        if (_hostDbModelsList != null && _apartmentDbModelsList != null) return;
        _hostDbModelsList = FakeStorage.HostDbModelsList.ToList();
        _apartmentDbModelsList = FakeStorage.ApartmentDbModelsList.ToList();
    }
    
    public List<ApartmentDBModel> GetApartmentsOfHost(int hostId)
    {
        LoadData();
        List<ApartmentDBModel> hostApartmentDbModels = new List<ApartmentDBModel>();
        foreach (var apartment in _apartmentDbModelsList)
        {
            if (apartment.HostId == hostId)
            {
                hostApartmentDbModels.Add(apartment);
            }
        }
        return hostApartmentDbModels;
    }
}