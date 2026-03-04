namespace BookingManagerDBModels;

public class ApartmentDBModel
{
    public int Id { get; }
    public int HostId { get; }
    public string Name { get; set; }
    public double SquareMeters { get; set; }
    public Currency PricePerNight { get; set; }
    public int MinimumStay  { get; set; }
}