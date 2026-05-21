using Models;

namespace StorageContext;

public interface IStorageContext
{
    IEnumerable<Host> GetAllHosts();
    void AddHost(Host host);
    Host GetHost(int hostId);
    Host GetHost(string name);
    void RemoveHost(int hostId);
    void UpdateHost(Host host);
    IEnumerable<Apartment> GetApartmentsOfHost(int hostId);
}