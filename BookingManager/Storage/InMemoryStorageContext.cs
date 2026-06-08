using Models;

namespace StorageContext;

public class InMemoryStorageContext
{
    private static List<Host> _hosts = new List<Host>();
    private static List<Apartment> _apartments = new List<Apartment>();
    
    static InMemoryStorageContext()
    {
        #region Fake Storage Context
        var privateHost = new Host
        {
            HostId = 1,
            FirstName = "Valentina",
            LastName = "Petrovna",
            Type = HostType.Private,
            Email = "valentinapetrovna@gmail.com",
            Phone = "+380671234567",
            DateOfBirth = new DateTime(1960, 7, 24)
        };

        var multiUnitHost = new Host
        {
            HostId = 2,
            FirstName = "Vasylii",
            LastName = "Temshik",
            Type = HostType.MultiUnit,
            Email = "yatemschik@gmail.com",
            Phone = "+380996767676",
            DateOfBirth = new DateTime(2000, 1, 1)
        };

        var agencyHost = new Host
        {
            HostId = 3,
            FirstName = "Vazhnyi",
            LastName = "Bymazhniy",
            Type = HostType.Agency,
            Email = "companyagent@gmail.com",
            Phone = "+38099001122",
            DateOfBirth = new DateTime(1991, 8, 24)
        };

        _hosts.Add(privateHost);
        _hosts.Add(multiUnitHost);
        _hosts.Add(agencyHost);

        var comnataUPetrovni = new Apartment
        {
            Id = 1,
            HostId = privateHost.HostId,
            Name = "Comnata V Babyli",
            Type = ApartmentType.PrivateRoom,
            SquareMeters = 10,
            PricePerNight = 30.00m,
            MinimumStay = 30,
            Rating = 5
        };
        _apartments.Add(comnataUPetrovni);

        var comnataSPetrovnoi = new Apartment
        {
            Id = 2,
            HostId = privateHost.HostId,
            Name = "Comnata Babyli",
            Type = ApartmentType.SharedRoom,
            SquareMeters = 20,
            PricePerNight = 20.00m, 
            MinimumStay = 365,
            Rating = 1
        };
        _apartments.Add(comnataSPetrovnoi);

        // Vasylii
        var vasyliiApt1 = new Apartment
        {
            Id = 3,
            HostId = multiUnitHost.HostId,
            Name = "Cozy Kyiv Center Studio",
            Type = ApartmentType.EntireApartment,
            SquareMeters = 35.5,
            PricePerNight = 1200.00m,
            MinimumStay = 2,
            Rating = 4.8
        };

        var vasyliiApt2 = new Apartment
        {
            Id = 4,
            HostId = multiUnitHost.HostId,
            Name = "Quiet Obolon Private Room",
            Type = ApartmentType.PrivateRoom,
            SquareMeters = 18.0,
            PricePerNight = 600.00m,
            MinimumStay = 1,
            Rating = 4.5
        };

        var vasyliiApt3 = new Apartment
        {
            Id = 5,
            HostId = multiUnitHost.HostId,
            Name = "Student Friendly Shared Dorm",
            Type = ApartmentType.SharedRoom,
            SquareMeters = 25.0,
            PricePerNight = 250.00m,
            MinimumStay = 1,
            Rating = 4.2
        };

        _apartments.Add(vasyliiApt1);
        _apartments.Add(vasyliiApt2);
        _apartments.Add(vasyliiApt3);

        // Agency
        var agencyApt1 = new Apartment
        {
            Id = 6,
            HostId = agencyHost.HostId,
            Name = "Luxury Pechersk Penthouse",
            Type = ApartmentType.EntireApartment,
            SquareMeters = 120.0,
            PricePerNight = 4500.00m,
            MinimumStay = 3,
            Rating = 5.0
        };

        var agencyApt2 = new Apartment
        {
            Id = 7,
            HostId = agencyHost.HostId,
            Name = "Modern Lviv Balcony Flat",
            Type = ApartmentType.EntireApartment,
            SquareMeters = 65.0,
            PricePerNight = 2200.00m,
            MinimumStay = 2,
            Rating = 4.9
        };

        var agencyApt3 = new Apartment
        {
            Id = 8,
            HostId = agencyHost.HostId,
            Name = "Khreschatyk Business Suite",
            Type = ApartmentType.EntireApartment,
            SquareMeters = 80.5,
            PricePerNight = 3500.00m,
            MinimumStay = 2,
            Rating = 4.7
        };

        var agencyApt4 = new Apartment
        {
            Id = 9,
            HostId = agencyHost.HostId,
            Name = "Odessa Sea Breeze Studio",
            Type = ApartmentType.EntireApartment,
            SquareMeters = 45.0,
            PricePerNight = 1800.00m,
            MinimumStay = 2,
            Rating = 4.8
        };

        var agencyApt5 = new Apartment
        {
            Id = 10,
            HostId = agencyHost.HostId,
            Name = "Premium Podil Private Suite",
            Type = ApartmentType.PrivateRoom,
            SquareMeters = 30.0,
            PricePerNight = 950.00m,
            MinimumStay = 1,
            Rating = 4.6
        };

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

    public IEnumerable<Apartment> GetAllApartments()
    {
        return _apartments;
    }
}