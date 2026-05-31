using Models.DTOs;

namespace Services.Host;

public interface IHostService
{
    List<HostListItemDTO> GetHostsList();
    HostDetailsDTO GetHost(int id);
    HostDetailsDTO GetHost(string name);
    bool RemoveHost(int id);
    void AddHost(HostCreateDTO host);
    void UpdateHost(HostDetailsDTO host);
    void SaveHosts();
    int GetHostsCount();
}