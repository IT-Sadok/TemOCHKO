namespace Repositories.Apartment;

public interface IApartmentRepository
{
    Task<IEnumerable<Models.Apartment>> GetApartmentsOfHostAsync(int hostId);
    Task<IEnumerable<Models.Apartment>> GetAllApartmentsAsync();
    Task SaveApartmentsAsync();
}