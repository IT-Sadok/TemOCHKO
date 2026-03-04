namespace BookingManagerDBModels;

public class HostDBModel
{
    public int Id { get; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public DateTime DateOfBirth { get; set; }
    public double AverageRating { get; set; }
}