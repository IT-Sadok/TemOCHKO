namespace Repositories.Apartment;

public interface IApartmentRepository
{
    IEnumerable<Models.Apartment> GetApartmentsOfHost(int hostId);
    IEnumerable<Models.Apartment> GetAllApartments();
    void SaveApartments();
}