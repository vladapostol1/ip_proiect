using System.Security.Claims;

public static class ClaimsPrincipalExtensions
{
    public static int GetRestaurantId(this ClaimsPrincipal user)
    {
        var restaurantIdClaim = user.Claims.FirstOrDefault(c => c.Type == "restaurant_id");
        return restaurantIdClaim != null ? int.Parse(restaurantIdClaim.Value) : 0;
    }
}