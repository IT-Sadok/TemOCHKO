using Repositories;
using Repositories.Apartment;
using Repositories.Host;
using Services;
using Services.Apartment;
using Services.Host;
using StorageContext;

namespace BookingManager;

class Program
{
    static void Main(string[] args)
    {
        IHostRepository hostRepository = new HostRepository();
        IApartmentRepository apartmentRepository = new ApartmentRepository();
        IHostService hostService = new HostService(hostRepository);
        IApartmentService apartmentService = new ApartmentService(apartmentRepository);
        Menu menu = new Menu(hostService, apartmentService);

        menu.ShowMenu();
    }
}