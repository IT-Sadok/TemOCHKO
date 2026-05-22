using System.Runtime.InteropServices.JavaScript;

namespace Models.DTOs;

public class HostDetailsDTO
{
    public int Id { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public HostType Type { get; }
    public string Email { get; }
    public string Phone { get; }
    public DateTime DateOfBirth { get; }

    public HostDetailsDTO(Host host) : this(host.Id, host.FirstName, host.LastName, host.Type, host.Email, host.Phone, host.DateOfBirth)
    {
        
    }
    
    public HostDetailsDTO(int id, string firstName, string lastName, HostType type, string email, string phone, DateTime dateOfBirth)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Type = type;
        Email = email;
        Phone = phone;
        DateOfBirth = dateOfBirth;
    }
}