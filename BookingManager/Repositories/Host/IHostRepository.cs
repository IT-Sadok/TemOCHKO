using Models.DTOs;

namespace Repositories.Host;

public interface IHostRepository
{
    List<Models.Host> GetHosts();
    void AddHost(HostCreateDTO host); 
    bool RemoveHost(int hostId);
    Models.Host GetHost(int hostId);
    Models.Host GetHost(string name);
    void UpdateHost(Models.Host host);
    void SaveHosts();
    int GetHostsCount();
}