using System.Text.Json;
using StorageContext;

namespace Repositories.Apartment;

public class ApartmentRepository : IApartmentRepository
{
    private static readonly string SFileName= "Files";
    private static readonly string SDataBasePath = Path.Combine(GetServicesDirectory(), SFileName);
    private List<Models.Apartment> _apartments = new List<Models.Apartment>();

    private void Init()
    {
        if (_apartments.Count > 0) return; 

        if (!File.Exists(ApartmentsFilePath()))
        {
            CreateMockStorage();
        }
        else
        {
            string apartmentsJson = File.ReadAllText(ApartmentsFilePath());
            _apartments = JsonSerializer.Deserialize<List<Models.Apartment>>(apartmentsJson) ?? new List<Models.Apartment>();
        }
    }
    
    private void CreateMockStorage()
    {
        InMemoryStorageContext memoryStorageContext = new InMemoryStorageContext();
        _apartments = memoryStorageContext.GetAllApartments().ToList();
        SaveApartments();
    }
    
    public IEnumerable<Models.Apartment> GetApartmentsOfHost(int hostId)
    {
        Init();
        return _apartments.Where(apartment => apartment.HostId == hostId);
    }
    
    public IEnumerable<Models.Apartment> GetAllApartments()
    {
        Init();
        return _apartments;
    }
    
    private static string? GetServicesDirectory()
    {
        string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        return Path.GetFullPath(Path.Combine(baseDir, @"..\..\..\"));
    }
    
    private string ApartmentsFilePath()
    {
        return Path.Combine(SDataBasePath, "Apartments.json");
    }
    
    public void SaveApartments()
    {
        Directory.CreateDirectory(SDataBasePath);
        string jsonString = JsonSerializer.Serialize(_apartments, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(ApartmentsFilePath(), jsonString);
    }
}