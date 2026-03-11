using BookingManager.Common.Enums;

namespace BookingManager.DBModels;

public class ApartmentDBModel
{
    private static int _instanceCount = 0;
    public int Id { get; private set;  }
    public int HostId { get; private set; }
    public string Name { get; set; }
    public ApartmentType Type { get; set; }
    public double SquareMeters { get; set; }
    public decimal PricePerNight { get; set; }
    public int MinimumStay  { get; set; }
    public double Rating { get; set; }

    public ApartmentDBModel(int hostId, string name, ApartmentType type, double squareMeters, decimal pricePerNight,
        int minimumStay, double rating)
    {
        Id = _instanceCount++;
        HostId = hostId;
        Name = name;
        Type = type;
        SquareMeters = squareMeters;
        PricePerNight = pricePerNight;
        MinimumStay = minimumStay;
        Rating = rating;
    }
}