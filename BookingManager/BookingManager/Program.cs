using Repositories;
using Services;
using StorageContext;

namespace BookingManager;

class Program
{
    private static InMemoryStorageContext storageContext = new InMemoryStorageContext();
    
    static void Main(string[] args)
    {
        IHostRepository hostRepository = new HostRepository(storageContext);
        IHostService hostService = new HostService(hostRepository);
        Menu menu = new Menu(hostService);

        menu.ShowMenuAsync();
    }
}