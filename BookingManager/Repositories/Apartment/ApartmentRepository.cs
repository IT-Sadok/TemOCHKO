using System.Text.Json;
using StorageContext;

namespace Repositories.Apartment;

public class ApartmentRepository : IApartmentRepository
{
    private static readonly string SFileName= "Files";
    private static readonly string SDataBasePath = Path.Combine(GetServicesDirectory(), SFileName);
    private List<Models.Apartment> _apartments = new List<Models.Apartment>();
    private int _instanceCounter = 0;
    private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
    
    public class FileWrapper
    {
        public int InstanceCounter { get; set; }
        public List<Models.Apartment> Apartments { get; set; }
    }

    private async Task Init()
    {
        await _semaphore.WaitAsync();
        try
        {
            if (_apartments.Count > 0) return;

            if (!File.Exists(ApartmentsFilePath()))
            {
                await CreateMockStorage();
            }
            else
            {
                string jsonData = await File.ReadAllTextAsync(ApartmentsFilePath());
                await using FileStream openStream = File.OpenRead(ApartmentsFilePath());
                var data = await JsonSerializer.DeserializeAsync<FileWrapper>(openStream,
                    new JsonSerializerOptions {IncludeFields = true});
                if (data != null)
                {
                    _instanceCounter = data.InstanceCounter;
                    _apartments = data.Apartments;
                }
            }
        }
        finally
        {
            _semaphore.Release();
        }
    }
    
    private async Task CreateMockStorage()
    {
        InMemoryStorageContext memoryStorageContext = new InMemoryStorageContext();
        _apartments = memoryStorageContext.GetAllApartments().ToList();
        _instanceCounter = _apartments.Count;
        await SaveApartmentsAsync();
    }
    
    public async Task<IEnumerable<Models.Apartment>> GetApartmentsOfHostAsync(int hostId)
    {
        await Init();
        return _apartments.Where(apartment => apartment.HostId == hostId);
    }
    
    public async Task<IEnumerable<Models.Apartment>> GetAllApartmentsAsync()
    {
        await Init();
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
    
    public async Task SaveApartmentsAsync()
    {
        Directory.CreateDirectory(SDataBasePath);
        var data = new FileWrapper()
        {
            InstanceCounter = _instanceCounter,
            Apartments = _apartments
        };
        string jsonString = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true, IncludeFields = true});
        await File.WriteAllTextAsync(ApartmentsFilePath(), jsonString);
    }
}