namespace BookingManagerWeb.Application.Business;

public class ApartmentNotFoundException : Exception
{
    public IDictionary<string, object?> Errors { get; }

    public ApartmentNotFoundException(string msg) : base(msg)
    {
        Errors = new Dictionary<string, object?>();
    }
    
    public ApartmentNotFoundException(string msg, IDictionary<string, object?> errors) : base(msg)
    {
        Errors = errors;
    }
}