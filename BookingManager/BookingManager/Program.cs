using Repositories;
using Services;
using StorageContext;

namespace BookingManager;

class Program
{
    private static IStorageContext storageContext = new JsonStorageContext();
    
    static void Main(string[] args)
    {
        IHostRepository hostRepository = new HostRepository(storageContext);
        IApartmentRepository apartmentRepository = new ApartmentRepository(storageContext);
        IHostService hostService = new HostService(hostRepository);
        IApartmentService apartmentService = new ApartmentService(apartmentRepository);
        Menu menu = new Menu(hostService, apartmentService);

        menu.ShowMenu();
    }
}