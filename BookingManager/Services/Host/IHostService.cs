using Models.DTOs;

namespace Services.Host;

public interface IHostService
{
    Task<List<HostListItemDTO>> GetHostsListAsync();
    Task<HostDetailsDTO> GetHostAsync(int id);
    Task<HostDetailsDTO> GetHostAsync(string name);
    Task<bool> RemoveHostAsync(int id);
    Task AddHostAsync(HostCreateDTO host);
    Task UpdateHostAsync(HostDetailsDTO host);
    Task SaveHostsAsync();
    Task<int> GetHostsCountAsync();
}