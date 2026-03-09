using System.Security.Cryptography;
using BookingManager.DBModels;

namespace BookingManager.Services;

public class StorageService
{
    private List<HostDBModel> _hostDbModelsList;
    private List<ApartmentDBModel> _apartmentDbModelsList;

    public void LoadData()
    {
        _hostDbModelsList = FakeStorage.HostDbModelsList.ToList();
        _apartmentDbModelsList = FakeStorage.ApartmentDbModelsList.ToList();
    }
    
    public List<ApartmentDBModel> GetApartmentsOfHost(int hostId)
    {
        // LoadData();
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

    public List<HostDBModel> GetAllHosts()
    {
        return  _hostDbModelsList;
    }
    
    // CRUD (For Host Entity)
    public void AddHost(HostDBModel host)
    {
        _hostDbModelsList.Add(host);
    }

    public bool RemoveHost(HostDBModel host)
    {
        return _hostDbModelsList.Remove(host);
      
    }

    public bool RemoveHost(int hostId)
    {
        foreach (var host in _hostDbModelsList)
        {
            if (host.Id == hostId)
            {
                _hostDbModelsList.Remove(host);
                return true;
            }
        }
        return false;
    }

    public bool UpdateHost(HostDBModel hostDbModel)
    {
        foreach (var host in _hostDbModelsList)
        {
            if (hostDbModel.Id == host.Id)
            {
                int indexInList = _hostDbModelsList.IndexOf(host);
                _hostDbModelsList.Remove(host);
                _hostDbModelsList.Insert(indexInList, hostDbModel);
                return true;
            }
        }
        return false;
    }

    private void SaveDataToStorage()
    {
        
    }
}