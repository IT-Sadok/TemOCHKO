using BookingManager.Common.Enums;
using BookingManager.DBModels;

namespace BookingManager.Services;

public static class FakeStorage
{
    private static List<HostDBModel> _hostDbModelsList;
    private static List<ApartmentDBModel> _apartmentDbModelsList;

    public static IReadOnlyList<HostDBModel> HostDbModelsList
    {
        get
        {
            return _hostDbModelsList.ToList();
        }
    }

    public static IReadOnlyList<ApartmentDBModel> ApartmentDbModelsList
    {
        get
        {
            return _apartmentDbModelsList.ToList();
        }
    }
    
    static FakeStorage()
    {
        _hostDbModelsList = new List<HostDBModel>();
        _apartmentDbModelsList = new List<ApartmentDBModel>();
        
        // Arrange
        // three example hosts
        var privateHost = new HostDBModel("Valentina", "Petrovna", HostType.Private, "valentinapetrovna@gmail.com", 
            "+380671234567", new DateTime(1960, 7, 24));
        var multiUnitHost = new HostDBModel("Vasylii", "Temshik", HostType.MultiUnit, "yatemschik@gmail.com",
            "+380996767676", new DateTime(2000, 1, 1));
        var agencyHost = new HostDBModel("Vazhnyi", "Bymazhniy", HostType.Agency, "companyagent@gmail.com",
            "+38099001122", new DateTime(1991, 8, 24));
        
        _hostDbModelsList.Add(privateHost);
        _hostDbModelsList.Add(multiUnitHost);
        _hostDbModelsList.Add(agencyHost);
        // 10 examples of apartments belonging to host 

        var comnataUPetrovni = new ApartmentDBModel(privateHost.Id, "Comnata V Babyli", ApartmentType.PrivateRoom, 10,
            30.00m, 30, 5);
        _apartmentDbModelsList.Add(comnataUPetrovni);
        var comnataSPetrovnoi = new ApartmentDBModel(privateHost.Id, "Comnata Babyli", ApartmentType.SharedRoom, 20, 20, 365, 1);
        _apartmentDbModelsList.Add(comnataSPetrovnoi);
        
        // Vasylii
        var vasyliiApt1 = new ApartmentDBModel(
            multiUnitHost.Id, 
            "Cozy Kyiv Center Studio", 
            ApartmentType.EntireApartment, 
            35.5, 
            1200.00m, 
            2, 
            4.8
        );

        var vasyliiApt2 = new ApartmentDBModel(
            multiUnitHost.Id, 
            "Quiet Obolon Private Room", 
            ApartmentType.PrivateRoom, 
            18.0, 
            600.00m, 
            1, 
            4.5
        );

        var vasyliiApt3 = new ApartmentDBModel(
            multiUnitHost.Id, 
            "Student Friendly Shared Dorm", 
            ApartmentType.SharedRoom, 
            25.0, 
            250.00m, 
            1, 
            4.2
        );
        
        _apartmentDbModelsList.Add(vasyliiApt1);
        _apartmentDbModelsList.Add(vasyliiApt2);
        _apartmentDbModelsList.Add(vasyliiApt3);
        
        // Agency
        var agencyApt1 = new ApartmentDBModel(
            agencyHost.Id, 
            "Luxury Pechersk Penthouse", 
            ApartmentType.EntireApartment, 
            120.0, 
            4500.00m, 
            3, 
            5.0
        );

        var agencyApt2 = new ApartmentDBModel(
            agencyHost.Id, 
            "Modern Lviv Balcony Flat", 
            ApartmentType.EntireApartment, 
            65.0, 
            2200.00m, 
            2, 
            4.9
        );

        var agencyApt3 = new ApartmentDBModel(
            agencyHost.Id, 
            "Khreschatyk Business Suite", 
            ApartmentType.EntireApartment, 
            80.5, 
            3500.00m, 
            2, 
            4.7
        );

        var agencyApt4 = new ApartmentDBModel(
            agencyHost.Id, 
            "Odessa Sea Breeze Studio", 
            ApartmentType.EntireApartment, 
            45.0, 
            1800.00m, 
            2, 
            4.8
        );

        var agencyApt5 = new ApartmentDBModel(
            agencyHost.Id, 
            "Premium Podil Private Suite", 
            ApartmentType.PrivateRoom, 
            30.0, 
            950.00m, 
            1, 
            4.6
        );
        
        _apartmentDbModelsList.Add(agencyApt1);
        _apartmentDbModelsList.Add(agencyApt2);
        _apartmentDbModelsList.Add(agencyApt3);
        _apartmentDbModelsList.Add(agencyApt4);
        _apartmentDbModelsList.Add(agencyApt5);
    }
}