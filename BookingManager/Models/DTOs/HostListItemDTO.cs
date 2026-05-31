namespace Models.DTOs;

public class HostListItemDTO
{
    public int Id { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public HostType Type { get; init; }
    public string Phone { get; init; }
}