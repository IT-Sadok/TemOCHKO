namespace BookingManagerWeb.Application.Business;

public class ApartmentOccupiedException : Exception
{
    public IDictionary<string, object?> Errors { get; }

    public ApartmentOccupiedException(string msg) : base(msg)
    {
        Errors = new Dictionary<string, object?>();
    }
    
    public ApartmentOccupiedException(string msg, IDictionary<string, object?> errors) : base(msg)
    {
        Errors = errors;
    }
}