namespace QuickServe.Interfaces;
public interface IRestaurantService
{
    int AddRestaurant(CreateRestaurantRecord createRestaurantRecord);
    RestaurantRecord GetRestaurantById(int id);
}
