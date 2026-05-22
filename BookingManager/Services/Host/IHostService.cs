using Models;
using Models.DTOs;

namespace Services;

public interface IHostService
{
    List<HostListDTO> GetHostsList();
    HostDetailsDTO GetHost(int id);
    HostDetailsDTO GetHost(string name);
    bool RemoveHost(int id);
    void AddHost(HostCreateDTO host);
    void UpdateHost(HostDetailsDTO host);
    void SaveHosts();
    int GetHostsCount();
}