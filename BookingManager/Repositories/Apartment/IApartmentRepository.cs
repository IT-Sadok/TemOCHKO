using Models;

namespace Repositories;

public interface IApartmentRepository
{
    IEnumerable<Apartment> GetApartmentsOfHost(int hostId);
    void SaveData();
}