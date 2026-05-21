using IdGenerator;
using Models;

namespace StorageContext;

public class InMemoryStorageContext : IStorageContext
{
    private static List<Host> _hosts = new List<Host>();
    private static List<Apartment> _apartments = new List<Apartment>();

    private static GeneratorId _idGenerator = new GeneratorId();
    
    static InMemoryStorageContext()
    {
        #region Fake Storage Context
        var privateHost = new Host(_idGenerator.GenerateHostId(), "Valentina", "Petrovna", HostType.Private, "valentinapetrovna@gmail.com", 
            "+380671234567", new DateTime(1960, 7, 24));
        var multiUnitHost = new Host(_idGenerator.GenerateHostId(), "Vasylii", "Temshik", HostType.MultiUnit, "yatemschik@gmail.com",
            "+380996767676", new DateTime(2000, 1, 1));
        var agencyHost = new Host(_idGenerator.GenerateHostId(), "Vazhnyi", "Bymazhniy", HostType.Agency, "companyagent@gmail.com",
            "+38099001122", new DateTime(1991, 8, 24));
        
        _hosts.Add(privateHost);
        _hosts.Add(multiUnitHost);
        _hosts.Add(agencyHost);

        var comnataUPetrovni = new Apartment(_idGenerator.GenerateApartmentId(), privateHost.Id, "Comnata V Babyli", ApartmentType.PrivateRoom, 10,
            30.00m, 30, 5);
        _apartments.Add(comnataUPetrovni);
        var comnataSPetrovnoi = new Apartment(_idGenerator.GenerateApartmentId(), privateHost.Id, "Comnata Babyli", ApartmentType.SharedRoom, 20, 20, 365, 1);
        _apartments.Add(comnataSPetrovnoi);
        
        // Vasylii
        var vasyliiApt1 = new Apartment(
            _idGenerator.GenerateApartmentId(), 
            multiUnitHost.Id, 
            "Cozy Kyiv Center Studio", 
            ApartmentType.EntireApartment, 
            35.5, 
            1200.00m, 
            2, 
            4.8
        );

        var vasyliiApt2 = new Apartment(
            _idGenerator.GenerateApartmentId(), 
            multiUnitHost.Id, 
            "Quiet Obolon Private Room", 
            ApartmentType.PrivateRoom, 
            18.0, 
            600.00m, 
            1, 
            4.5
        );

        var vasyliiApt3 = new Apartment(
            _idGenerator.GenerateApartmentId(), 
            multiUnitHost.Id, 
            "Student Friendly Shared Dorm", 
            ApartmentType.SharedRoom, 
            25.0, 
            250.00m, 
            1, 
            4.2
        );
        
        _apartments.Add(vasyliiApt1);
        _apartments.Add(vasyliiApt2);
        _apartments.Add(vasyliiApt3);
        
        // Agency
        var agencyApt1 = new Apartment(
            _idGenerator.GenerateApartmentId(), 
            agencyHost.Id, 
            "Luxury Pechersk Penthouse", 
            ApartmentType.EntireApartment, 
            120.0, 
            4500.00m, 
            3, 
            5.0
        );

        var agencyApt2 = new Apartment(
            _idGenerator.GenerateApartmentId(), 
            agencyHost.Id, 
            "Modern Lviv Balcony Flat", 
            ApartmentType.EntireApartment, 
            65.0, 
            2200.00m, 
            2, 
            4.9
        );

        var agencyApt3 = new Apartment(
            _idGenerator.GenerateApartmentId(), 
            agencyHost.Id, 
            "Khreschatyk Business Suite", 
            ApartmentType.EntireApartment, 
            80.5, 
            3500.00m, 
            2, 
            4.7
        );

        var agencyApt4 = new Apartment(
            _idGenerator.GenerateApartmentId(), 
            agencyHost.Id, 
            "Odessa Sea Breeze Studio", 
            ApartmentType.EntireApartment, 
            45.0, 
            1800.00m, 
            2, 
            4.8
        );

        var agencyApt5 = new Apartment(
            _idGenerator.GenerateApartmentId(), 
            agencyHost.Id, 
            "Premium Podil Private Suite", 
            ApartmentType.PrivateRoom, 
            30.0, 
            950.00m, 
            1, 
            4.6
        );
        
        _apartments.Add(agencyApt1);
        _apartments.Add(agencyApt2);
        _apartments.Add(agencyApt3);
        _apartments.Add(agencyApt4);
        _apartments.Add(agencyApt5);
        
        #endregion
    }
    
    public IEnumerable<Host> GetAllHosts()
    {
        return _hosts;
    }

    public void AddHost(Host host)
    {
        _hosts.Add(host);
    }

    public Host GetHost(int hostId)
    {
        return _hosts.FirstOrDefault(host => host.Id == hostId);
    }

    public Host GetHost(string name)
    {
        return _hosts.FirstOrDefault(host => host.FirstName.ToLower() + " " + host.LastName.ToLower() == name.ToLower());
    }

    public bool RemoveHost(int hostId)
    {
        var hostToRemove = _hosts.FirstOrDefault(host => host.Id == hostId);
        if (hostToRemove != null)
            return _hosts.Remove(hostToRemove); 
        return false; 
    }

    public void UpdateHost(Host host)
    {
        var index = _hosts.IndexOf(host);
        if (index >= 0 && index < _hosts.Count)
            _hosts[index] = host;
    }

    public IEnumerable<Apartment> GetApartmentsOfHost(int hostId)
    {
        return _apartments.Where(apartment => apartment.HostId == hostId);
    }
}