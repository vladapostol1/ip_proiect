namespace QuickServe.Interfaces;

using QuickServe.Models;

public interface IFoodService
{
    Task<IEnumerable<FoodModel>> GetFoodsAsync(int restaurantId);
    Task<FoodModel> GetFoodByIdAsync(int id, int restaurantId);
    Task AddFoodAsync(FoodModel food);
    Task DeleteFoodAsync(int id, int restaurantId);
}
