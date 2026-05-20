namespace Models;

public class Host
{
    public int Id { get; private set;  }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public HostType Type { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public DateTime DateOfBirth { get; set; }

    public Host(int hostId, string firstName, string lastName, HostType type,  string email, string phone,
        DateTime dateOfBirth)
    {
        Id = hostId;
        FirstName = firstName;
        LastName = lastName;
        Type = type;
        Email = email;
        Phone = phone;
        DateOfBirth = dateOfBirth;
    }
}