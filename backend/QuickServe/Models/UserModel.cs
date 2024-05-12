namespace QuickServe.Models;

public class UserModel
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public int RestaurantId { get; set; }
}
