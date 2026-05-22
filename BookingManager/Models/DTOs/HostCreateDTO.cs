namespace Models.DTOs;

public class HostCreateDTO
{
    public string FirstName { get; }
    public string LastName { get; }
    public HostType Type { get; }
    public string Email { get; }
    public string Phone { get; }
    public DateTime DateOfBirth { get; }

    public HostCreateDTO(string firstName, string lastName, HostType type,  string email, string phone,
        DateTime dateOfBirth)
    {
        FirstName = firstName;
        LastName = lastName;
        Type = type;
        Email = email;
        Phone = phone;
        DateOfBirth = dateOfBirth;
    }
}