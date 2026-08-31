namespace BookingManagerWeb.Domain.Constants;

public static class BookingStatus
{
    public const string Pending = "Pending";
    public const string Confirmed = "Confirmed";
    public const string Canceled = "Canceled";
    
    public static IReadOnlyCollection<string> All => [Pending, Confirmed, Canceled];
}