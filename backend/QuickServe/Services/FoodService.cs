namespace QuickServe.Services;

using QuickServe.Models;
using QuickServe.Interfaces;
using System.Data.SQLite;
using Dapper;
using Microsoft.Extensions.Configuration;

public class FoodService : IFoodService
{
    private readonly string _connectionString;

    public FoodService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("SQLiteConnection");
    }

    public async Task<IEnumerable<FoodModel>> GetFoodsAsync(int restaurantId)
    {
        using var connection = new SQLiteConnection(_connectionString);
        var foods = await connection.QueryAsync<FoodModel>("SELECT * FROM foods WHERE restaurant_id = @RestaurantId", new { RestaurantId = restaurantId });
        return foods;
    }

    public async Task<FoodModel> GetFoodByIdAsync(int id, int restaurantId)
    {
        using var connection = new SQLiteConnection(_connectionString);
        var food = await connection.QueryFirstOrDefaultAsync<FoodModel>("SELECT * FROM foods WHERE id = @Id AND restaurant_id = @RestaurantId", new { Id = id, RestaurantId = restaurantId });
        return food;
    }

    public async Task AddFoodAsync(FoodModel food)
    {
        using var connection = new SQLiteConnection(_connectionString);
        await connection.ExecuteAsync("INSERT INTO foods (name, price, description, restaurant_id) VALUES (@Name, @Price, @Description, @RestaurantId)", food);
    }

    public async Task DeleteFoodAsync(int id, int restaurantId)
    {
        using var connection = new SQLiteConnection(_connectionString);
        await connection.ExecuteAsync("DELETE FROM foods WHERE id = @Id AND restaurant_id = @RestaurantId", new { Id = id, RestaurantId = restaurantId });
    }
}