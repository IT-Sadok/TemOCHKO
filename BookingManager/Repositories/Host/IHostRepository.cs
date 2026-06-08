using Models.DTOs;

namespace Repositories.Host;

public interface IHostRepository
{
    Task<List<Models.Host>> GetHostsAsync();
    Task AddHostAsync(HostCreateDTO host); 
    Task<bool> RemoveHostAsync(int hostId);
    Task<Models.Host> GetHostAsync(int hostId);
    Task<Models.Host> GetHostAsync(string name);
    Task UpdateHostAsync(Models.Host host);
    Task SaveHostsAsync();
    Task<int> GetHostsCountAsync();
}