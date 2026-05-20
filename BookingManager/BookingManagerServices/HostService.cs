using BookingManager.DBModels;
using BookingManager.MainApp;

namespace BookingManager.Services;

public class HostService
{
    private StorageContext _context;
    private ApartmentService _apartmentService;

    public HostService(StorageContext storageContext)
    {
        _context = storageContext;
        _apartmentService = new ApartmentService(_context);
    }
    
    public List<HostDTO> GetAllHosts()
    {
        var list = new List<HostDTO>();

        foreach (var dbHost in _context.GetAllHosts())
        {
            var host = new HostDTO(dbHost);
            host.LoadApartments(_apartmentService);
            list.Add(host);
        }

        return list;
    }
    
    // CRUD (For Host Entity)
    public void AddHost(HostDBModel host)
    {
        _context.AddHost(host);
    }

    public bool RemoveHost(int hostId)
    {
        foreach (var host in _context.GetAllHosts().ToList())
        {
            if (host.Id == hostId)
            {
                _context.RemoveHost(hostId);
                return true;
            }
        }
        return false;
    }

    public bool UpdateHost(HostDTO hostDto)
    {
        var hostList = _context.GetAllHosts().ToList();
        foreach (var host in hostList)
        {
            if (hostDto.Id == host.Id)
            {
                int indexInList = hostList.IndexOf(host);
                _context.RemoveHost(host.Id);
                _context.AddHostAtIndex(indexInList, new HostDBModel(host.FirstName, host.LastName, host.Type, host.Email, host.Phone, host.DateOfBirth));
                return true;
            }
        }
        return false;
    }
    
    private int FindHostIndexByName(string hostName)
    {
        hostName = hostName.ToLower();
        hostName = hostName.Trim();
        var hostList = _context.GetAllHosts().ToList();
        foreach (var host in hostList)
        {
            if ((host.FirstName.ToLower() + " " + host.LastName.ToLower()) == hostName)
            {
                return hostList.IndexOf(host);
            }
        }
        return -1;
    }

    public HostDTO FindHostByName(string hostName)
    {
        hostName = hostName.ToLower();
        hostName = hostName.Trim();
        foreach (var host in _context.GetAllHosts().ToList())
        {
            if ((host.FirstName.ToLower() + " " + host.LastName.ToLower()) == hostName)
            {
                return new HostDTO(host.Id, host.FirstName, host.LastName, host.Type, host.Email, host.Phone, host.DateOfBirth);
            }
        }
        return null;
    }

    public HostDTO FindHostById(int hostId)
    {
        foreach (var host in _context.GetAllHosts().ToList())
        {
            if (host.Id == hostId)
                return new HostDTO(host.Id, host.FirstName, host.LastName, host.Type, host.Email, host.Phone, host.DateOfBirth);
        }

        return null;
    }

    public bool ValidateHost(HostDTO host)
    {
        if (host is null)
            return false;

        var wholeName = host.FirstName.ToLower() + " " + host.LastName.ToLower();
        if (Common.Tools.Common.IsNameReserved(wholeName))
            return false;

        if (IsNameDuplicate(wholeName))
            return false;
        
        return true;
    }
    
    private bool IsNameDuplicate(string fullName)
    {
        fullName = fullName.ToLower();
        fullName = fullName.Trim();
        foreach (var host in _context.GetAllHosts().ToList())
        {
            if ((host.FirstName.ToLower() + " " + host.LastName.ToLower()) == fullName)
            {
                return true;
            }
        }
        return false;
    }
}