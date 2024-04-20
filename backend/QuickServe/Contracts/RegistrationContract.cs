namespace QuickServe.Contracts;

public record class RegistrationContract
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string RestaurantName { get; set; } = string.Empty;
    public string RestaurantAddress { get; set; } = string.Empty;
    public string RestaurantWebsite { get; set; } = string.Empty;
}
