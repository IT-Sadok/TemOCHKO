namespace BookingManagerWeb.Domain.Constants;

public static class Roles
{
    public const string Client = "Client";
    public const string Host = "Host";
    public const string User = "User";
    
    public static IReadOnlyCollection<string> All => [ Client, Host, User ];
}