using System.Runtime.CompilerServices;
using System.Text.Json;
using Models;

namespace StorageContext;

public class JsonStorageContext : IStorageContext
{
    private static readonly string FileName= "Files";
    private static readonly string DataBasePath = Path.Combine(GetServicesDirectory(), FileName);
    
    private List<Host> _hosts = new List<Host>();
    private List<Apartment> _apartments = new List<Apartment>();
    
    private void Init()
    {
        if (_hosts.Count > 0 || _apartments.Count > 0) return; 

        if (!File.Exists(HostsFilePath()) || !File.Exists(ApartmentsFilePath()))
        {
            CreateMockStorage();
        }
        else
        {
            string hostsJson = File.ReadAllText(HostsFilePath());
            _hosts = JsonSerializer.Deserialize<List<Host>>(hostsJson) ?? new List<Host>();
            
            string apartmentsJson = File.ReadAllText(ApartmentsFilePath());
            _apartments = JsonSerializer.Deserialize<List<Apartment>>(apartmentsJson) ?? new List<Apartment>();
        }
        
    }

    private void CreateMockStorage()
    {
        InMemoryStorageContext memoryStorageContext = new InMemoryStorageContext();
        _hosts = memoryStorageContext.GetAllHosts().ToList();
        _apartments = memoryStorageContext.GetAllApartments().ToList();
        SaveHosts();
        SaveApartments();
    }
    
    public IEnumerable<Host> GetAllHosts()
    {
        Init();
        return _hosts;
    }

    public IEnumerable<Apartment> GetAllApartments()
    {
        Init();
        return _apartments;
    }

    public void AddHost(Host host)
    {
        Init();
        _hosts.Add(host);
    }

    public Host GetHost(int hostId)
    {
        Init();
        return _hosts.FirstOrDefault(h => h.HostId == hostId);
    }

    public Host GetHost(string name)
    {
        Init();
        return _hosts.FirstOrDefault(host => host.FirstName.ToLower() + " " + host.LastName.ToLower() == name);
    }

    public bool RemoveHost(int hostId)
    {
        Init();
        return _hosts.RemoveAll(h => h.HostId == hostId) > 0;
    }

    public void UpdateHost(Host host)
    {
        Init();
        for (int i = 0; i < _hosts.Count; i++)  
            if (_hosts[i].HostId == host.HostId) _hosts[i] = host;
    }

    public IEnumerable<Apartment> GetApartmentsOfHost(int hostId)
    {
        Init();
        return _apartments.Where(apartment => apartment.HostId == hostId);

    }

    public void SaveHosts()
    {
        Directory.CreateDirectory(DataBasePath);
        string jsonString = JsonSerializer.Serialize(_hosts, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(HostsFilePath(), jsonString);
    }

    public void SaveApartments()
    {
        Directory.CreateDirectory(DataBasePath);
        string jsonString = JsonSerializer.Serialize(_apartments, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(ApartmentsFilePath(), jsonString);
    }

    public int GetHostsCount()
    {
        return _hosts.Count;
    }

    // gets path to BookingManagerServices project
    public static string? GetServicesDirectory()
    {
        string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        return Path.GetFullPath(Path.Combine(baseDir, @"..\..\..\"));
    }
    
    private string HostsFilePath()
    {
        return Path.Combine(DataBasePath, "Hosts.json");
    }
    
    private string ApartmentsFilePath()
    {
        return Path.Combine(DataBasePath, "Apartments.json");
    }
}