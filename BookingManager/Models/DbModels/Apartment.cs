namespace Models;

public class Apartment
{
    public int Id { get; set; }
    public int HostId { get; set; }
    public string Name { get; set; }
    public ApartmentType Type { get; set; }
    public double SquareMeters { get; set; }
    public decimal PricePerNight { get; set; }
    public int MinimumStay { get; set; }
    public double Rating { get; set; }
}