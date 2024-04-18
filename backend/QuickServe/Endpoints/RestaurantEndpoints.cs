namespace QuickServe.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using QuickServe.Interfaces;

public static class RestaurantEndpoints
{
    public static void MapRestaurantEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/restaurants", CreateRestaurant);
        app.MapGet("/restaurants/{id}", GetRestaurant);
    }

    private static async Task<IResult> CreateRestaurant(HttpRequest req, IRestaurantService restaurantService)
    {
        var createRestaurantRecord = await req.ReadFromJsonAsync<CreateRestaurantRecord>();
        if (createRestaurantRecord == null)
        {
            return Results.BadRequest("Invalid restaurant data");
        }

        var restaurantId = restaurantService.AddRestaurant(createRestaurantRecord);
        return Results.Created($"/restaurants/{restaurantId}", createRestaurantRecord);
    }

    private static IResult GetRestaurant(int id, IRestaurantService restaurantService)
    {
        var restaurant = restaurantService.GetRestaurantById(id);
        if (restaurant == null)
        {
            return Results.NotFound();
        }
        return Results.Ok(restaurant);
    }
}