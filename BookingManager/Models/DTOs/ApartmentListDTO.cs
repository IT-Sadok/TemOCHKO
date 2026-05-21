namespace Models.DTOs;

public class ApartmentListDTO
{
    public int Id { get; }
    public int HostId { get; }
    public string Name { get; }
    public ApartmentType Type { get; }
    public decimal PricePerNight { get; }
    public double Rating { get; }

    public ApartmentListDTO(Apartment apartment) : this(apartment.Id, apartment.HostId, apartment.Name, apartment.Type, apartment.PricePerNight, apartment.Rating)
    {
        
    }

    public ApartmentListDTO(int apartmentId, int hostId, string name, ApartmentType type, decimal pricePerNight, double rating)
    {
        Id = apartmentId;
        HostId = hostId;
        Name = name;
        Type = type;
        PricePerNight = pricePerNight;
        Rating = rating;
    }
}