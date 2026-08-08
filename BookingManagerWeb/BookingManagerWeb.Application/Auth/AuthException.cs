namespace BookingManagerWeb.Application.Auth;

public class AuthException : Exception
{
    public IDictionary<string, object?> Errors { get; } 

    public AuthException(string msg, IDictionary<string, object?> errors) : base(msg)
    {
        Errors = errors;
    }
}