namespace Models.DTOs;

public class ApartmentListItemDTO
{
    public int Id { get; init; }
    public int HostId { get; init; }
    public string Name { get; init; }
    public ApartmentType Type { get; init; }
    public decimal PricePerNight { get; set; }
    public double Rating { get; init; }
    
}