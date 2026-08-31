namespace BookingManagerWeb.Domain.Exceptions;

public class AppBaseException : Exception
{
    public int StatusCode { get; }
    public string Title { get; }

    protected AppBaseException(string message, int statusCode, string title) 
        : base(message)
    {
        StatusCode = statusCode;
        Title = title;
    }
}