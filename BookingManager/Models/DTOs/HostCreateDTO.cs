namespace Models.DTOs;

public class HostCreateDTO
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public HostType Type { get; init; }
    public string Email { get; init; }
    public string Phone { get; init; }
    public DateTime DateOfBirth { get; init; }
}