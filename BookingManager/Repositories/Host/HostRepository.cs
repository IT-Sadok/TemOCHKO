using System.Text.Json;
using Models.DTOs;
using StorageContext;

namespace Repositories.Host;

public class HostRepository :  IHostRepository
{
    private static readonly string s_fileName= "Files";
    private static readonly string s_dataBasePath = Path.Combine(GetServicesDirectory(), s_fileName);
    private List<Models.Host> _hosts = new List<Models.Host>();
    private int _instanceCounter = 0;
    private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
    
    public class FileWrapper
    {
        public int InstanceCounter { get; set; }
        public List<Models.Host> Hosts { get; set; }
    }
    
    private async Task Init()
    {
        await _semaphore.WaitAsync();
        try
        {
            if (_hosts.Count > 0) return;

            if (!File.Exists(HostsFilePath()))
            {
                await CreateMockStorageAsync();
            }
            else
            {
                string jsonData = await File.ReadAllTextAsync(HostsFilePath());
                await using FileStream openStream = File.OpenRead(HostsFilePath());
                var data = await JsonSerializer.DeserializeAsync<FileWrapper>(openStream,
                    new JsonSerializerOptions {IncludeFields = true});
                if (data != null)
                {
                    _instanceCounter = data.InstanceCounter;
                    _hosts = data.Hosts;
                }
            }
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<List<Models.Host>> GetHostsAsync()
    {
        await Init();
        return _hosts;
    } 
    
    public async Task AddHostAsync(HostCreateDTO host)
    {
        await Init();
        var hostDb = new Models.Host 
        {
            HostId = ++_instanceCounter,
            FirstName = host.FirstName,
            LastName = host.LastName, 
            Email =  host.Email, 
            Phone = host.Phone, 
            DateOfBirth =  host.DateOfBirth, 
            Type = host.Type
        };
        _hosts.Add(hostDb);
    }

    public async Task<bool> RemoveHostAsync(int hostId)
    {
        await Init();
        return _hosts.RemoveAll(h => h.HostId == hostId) > 0;
    }

    public async Task<Models.Host> GetHostAsync(int hostId)
    {
        await Init();
        return _hosts.FirstOrDefault(h => h.HostId == hostId);
    }

    public async Task<Models.Host> GetHostAsync(string name)
    {
        await Init();
        return _hosts.FirstOrDefault(host => host.FirstName.ToLower() + " " + host.LastName.ToLower() == name);
    }

    public async Task UpdateHostAsync(Models.Host host)
    {
        await Init();
        for (int i = 0; i < _hosts.Count; i++)  
            if (_hosts[i].HostId == host.HostId) _hosts[i] = host;
    }
    
    public async Task<int> GetHostsCountAsync()
    {
        await Init();
        return _hosts.Count;
    }
    
    private async Task CreateMockStorageAsync()
    {
        InMemoryStorageContext context = new InMemoryStorageContext();
        _hosts = context.GetAllHosts().ToList();
        _instanceCounter = _hosts.Count;
        await SaveHostsAsync();
    }
    
    public async Task SaveHostsAsync()
    {
        Directory.CreateDirectory(s_dataBasePath);
        var data = new FileWrapper
        {
            InstanceCounter = _instanceCounter,
            Hosts = _hosts
        };
        
        var options = new JsonSerializerOptions 
        { 
            WriteIndented = true,
            IncludeFields = true 
        };
        
        string jsonString =  JsonSerializer.Serialize(data, options);
        await File.WriteAllTextAsync(HostsFilePath(), jsonString);
    }
    
    private static string? GetServicesDirectory()
    {
        string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        return Path.GetFullPath(Path.Combine(baseDir, @"..\..\..\"));
    }
    
    private string HostsFilePath()
    {
        return Path.Combine(s_dataBasePath, "Hosts.json");
    }
    
}