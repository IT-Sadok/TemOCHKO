using Models;
using StorageContext;

namespace Repositories;

public class ApartmentRepository : IApartmentRepository
{
    private IStorageContext _storageContext;
    public ApartmentRepository(IStorageContext storageContext)
    {
        _storageContext = storageContext;
    }
    public IEnumerable<Apartment> GetApartmentsOfHost(int hostId)
    {
        return _storageContext.GetApartmentsOfHost(hostId);
    }

    public void SaveData()
    {
        _storageContext.SaveApartments();
    }
}