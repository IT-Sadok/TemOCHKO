using System.ComponentModel;
using System.Runtime.InteropServices.JavaScript;

namespace Models.DTOs;

public class HostDetailsDTO
{
    public int Id { get; init; }
    [DisplayName ("First Name")]
    public string FirstName { get; init; }
    [DisplayName ("Last Name")]
    public string LastName { get; init; }
    public HostType Type { get; init; }
    public string Email { get; init; }
    public string Phone { get; init; }
    [DisplayName("Date Of Birth")]
    public DateTime DateOfBirth { get; init; }
}