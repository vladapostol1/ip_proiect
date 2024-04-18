namespace QuickServe.Services;

using QuickServe.Contracts;
using QuickServe.Interfaces;
public class RestaurantService : IRestaurantService
{
    private readonly ApplicationDbContext _context;

    public RestaurantService(ApplicationDbContext context)
    {
        _context = context;
    }

    public int AddRestaurant(CreateRestaurantRecord createRestaurantRecord)
    {
        var restaurant = new Restaurant
        {
            Name = createRestaurantRecord.Name,
            Address = createRestaurantRecord.Address,
            Website = createRestaurantRecord.Website,
        };
        _context.Restaurants.Add(restaurant);
        _context.SaveChanges();
        return restaurant.Id;
    }

    public RestaurantRecord GetRestaurantById(int id)
    {
        var restaurant = _context.Restaurants.Find(id);
        if (restaurant == null)
        {
            return null;
        }

        return new RestaurantRecord(
            restaurant.Id,
            restaurant.Name,
            restaurant.Address,
            restaurant.Website,
            restaurant.MenuItems.Select(mi => new FoodRecord(mi.Id, mi.Name, mi.Category, mi.Description, mi.Price, mi.ImagePath)).ToList()
        );
    }
}
