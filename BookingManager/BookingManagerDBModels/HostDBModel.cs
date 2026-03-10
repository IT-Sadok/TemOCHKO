using BookingManager.Common.Enums;
namespace BookingManager.DBModels;

public class HostDBModel
{
    private static int _instanceCounter = 0;
    public int Id { get; private set;  }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public HostType Type { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public DateTime DateOfBirth { get; set; }

    public HostDBModel(string firstName, string lastName, HostType type,  string email, string phone,
        DateTime dateOfBirth)
    {
        Id = ++_instanceCounter;
        FirstName = firstName;
        LastName = lastName;
        Type = type;
        Email = email;
        Phone = phone;
        DateOfBirth = dateOfBirth;
    }
    
    public void ChangeId(int newId)
    {
        Id = newId;
    }
}