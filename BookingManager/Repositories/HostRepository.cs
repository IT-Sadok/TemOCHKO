using IdGenerator;
using Models;
using StorageContext;

namespace Repositories;

public class HostRepository :  IHostRepository
{
    private readonly IStorageContext _storageContext;
    
    public HostRepository(IStorageContext storageContext)
    {
        _storageContext =  storageContext;
    }

    public List<Host> GetHosts()
    {
        return _storageContext.GetAllHosts().ToList();
    }    
    
    public void AddHost(Host host)
    {
        _storageContext.AddHost(host);
    }

    public void RemoveHost(int hostId)
    {
        _storageContext.RemoveHost(hostId);
    }

    public Host GetHost(int hostId)
    {
        return _storageContext.GetHost(hostId);
    }

    public Host GetHost(string name)
    {
        return _storageContext.GetHost(name);
    }
}