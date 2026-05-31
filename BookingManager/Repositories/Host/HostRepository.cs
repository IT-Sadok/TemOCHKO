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
    
    private class FileWrapper
    {
        internal int InstanceCounter { get; set; }
        internal List<Models.Host> Hosts { get; set; }
    }
    
    private void Init()
    {
        if (_hosts.Count > 0) return; 

        if (!File.Exists(HostsFilePath()))
        {
            CreateMockStorage();
        }
        else
        {
            string jsonData = File.ReadAllText(HostsFilePath());
            var data = JsonSerializer.Deserialize<FileWrapper>(jsonData);
            if (data != null)
            {
                _instanceCounter = data.InstanceCounter;
                _hosts = data.Hosts;
            }
            else
            {
                _instanceCounter = 0;
                _hosts = new List<Models.Host>();
            }
        }
    }

    public List<Models.Host> GetHosts()
    {
        Init();
        return _hosts;
    } 
    
    public void AddHost(HostCreateDTO host)
    {
        Init();
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

    public bool RemoveHost(int hostId)
    {
        Init();
        return _hosts.RemoveAll(h => h.HostId == hostId) > 0;
    }

    public Models.Host GetHost(int hostId)
    {
        Init();
        return _hosts.FirstOrDefault(h => h.HostId == hostId);
    }

    public Models.Host GetHost(string name)
    {
        Init();
        return _hosts.FirstOrDefault(host => host.FirstName.ToLower() + " " + host.LastName.ToLower() == name);
    }

    public void UpdateHost(Models.Host host)
    {
        Init();
        for (int i = 0; i < _hosts.Count; i++)  
            if (_hosts[i].HostId == host.HostId) _hosts[i] = host;
    }
    
    public int GetHostsCount()
    {
        Init();
        return _hosts.Count;
    }
    
    private void CreateMockStorage()
    {
        InMemoryStorageContext context = new InMemoryStorageContext();
        _hosts = context.GetAllHosts().ToList();
        _instanceCounter = _hosts.Count;
        SaveHosts();
    }
    
    public void SaveHosts()
    {
        Directory.CreateDirectory(s_dataBasePath);
        var data = new FileWrapper
        {
            InstanceCounter = _instanceCounter,
            Hosts = _hosts
        };
        string jsonString = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(HostsFilePath(), jsonString);
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