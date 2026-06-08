using System.ComponentModel;

namespace Models;

public class Host
{
    public int HostId { get; set;  }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public HostType Type { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public DateTime DateOfBirth { get; set; }
}