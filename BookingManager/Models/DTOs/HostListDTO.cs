namespace Models.DTOs;

public class HostListDTO
{
    public int Id { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public HostType Type { get; set;}
    public string Phone { get; set; }

    public HostListDTO(Host host) : this(host.Id, host.FirstName, host.LastName, host.Type, host.Phone)
    {
        
    }
    
    public HostListDTO(int  id, string firstName, string lastName, HostType type, string phone)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Type = type;
        Phone = phone;
    }
}