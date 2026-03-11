using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text.Json;
using BookingManager.DBModels;

namespace BookingManager.Services;

public class StorageService
{
    private List<HostDBModel> _hostDbModelsList;
    private List<ApartmentDBModel> _apartmentDbModelsList;
    
    private static readonly string FileDbNameS = "Storage.json";
    private static readonly string FilePathS = Path.Combine(GetServicesDirectory(), FileDbNameS);
    
    public List<ApartmentDBModel> GetApartmentsOfHost(int hostId)
    {
        // LoadData();
        List<ApartmentDBModel> hostApartmentDbModels = new List<ApartmentDBModel>();
        foreach (var apartment in _apartmentDbModelsList)
        {
            if (apartment.HostId == hostId)
            {
                hostApartmentDbModels.Add(apartment);
            }
        }
        return hostApartmentDbModels;
    }

    public List<HostDBModel> GetAllHosts()
    {
        return  _hostDbModelsList;
    }
    
    // CRUD (For Host Entity)
    public void AddHost(HostDBModel host)
    {
        _hostDbModelsList.Add(host);
    }

    public bool RemoveHost(int hostId)
    {
        foreach (var host in _hostDbModelsList)
        {
            if (host.Id == hostId)
            {
                _hostDbModelsList.Remove(host);
                return true;
            }
        }
        return false;
    }

    public bool UpdateHost(HostDBModel hostDbModel)
    {
        foreach (var host in _hostDbModelsList)
        {
            if (hostDbModel.Id == host.Id)
            {
                int indexInList = _hostDbModelsList.IndexOf(host);
                _hostDbModelsList.Remove(host);
                _hostDbModelsList.Insert(indexInList, hostDbModel);
                return true;
            }
        }
        return false;
    }
    
    // Wrapper used to wrap two lists into one entity to store in a Json file
    private class FileWrapper
    { 
        public List<HostDBModel> Hosts { get; set; } = new();
        public List<ApartmentDBModel> Apartments { get; set; } = new();
    }

    public void SaveDataToFile()
    {
        var data = new FileWrapper()
        {
            Hosts = _hostDbModelsList,
            Apartments = _apartmentDbModelsList
        };
        
        string jsonString = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(FilePathS, jsonString);
    }
    
    public void LoadDataFromFile()
    {
        // Old way 
        /*_hostDbModelsList = FakeStorage.HostDbModelsList.ToList();
        _apartmentDbModelsList = FakeStorage.ApartmentDbModelsList.ToList();*/

        if (!File.Exists(FilePathS)) return;
        
        string jsonString = File.ReadAllText(FilePathS);
        var loadedData =  JsonSerializer.Deserialize<FileWrapper>(jsonString, new JsonSerializerOptions { WriteIndented = true });
        
        if (loadedData != null)
        {
            _hostDbModelsList = loadedData.Hosts;
            _apartmentDbModelsList = loadedData.Apartments;
        }
    }
    
    // gets path to BookingManagerServices project
    public static string? GetServicesDirectory([CallerFilePath] string filePath = "")
    {
        return Path.GetDirectoryName(filePath);
    }
}