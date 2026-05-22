using Models;

namespace Repositories;

public interface IHostRepository
{
    List<Host> GetHosts();
    void AddHost(Host host); 
    bool RemoveHost(int hostId);
    Host GetHost(int hostId);
    Host GetHost(string name);
    void UpdateHost(Host host);
}