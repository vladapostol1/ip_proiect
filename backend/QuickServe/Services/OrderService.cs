namespace QuickServe.Services;

using QuickServe.Models;
using QuickServe.Interfaces;
using System.Data.SQLite;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

public class OrderService : IOrderService
{
    private readonly string _connectionString;

    public OrderService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("SQLiteConnection");
    }

    public async Task<int> CreateOrderAsync(OrderModel order, List<OrderItemModel> items)
    {
        using var connection = new SQLiteConnection(_connectionString);
        await connection.OpenAsync();
        using var transaction = await connection.BeginTransactionAsync();

        try
        {
            var orderId = await connection.ExecuteScalarAsync<int>(
                "INSERT INTO orders (restaurant_id, table_number, total, order_time) VALUES (@RestaurantId, @TableNumber, @Total, @OrderTime); SELECT last_insert_rowid();",
                order, transaction);

            foreach (var item in items)
            {
                var foodId = item.Food.Id;

                var foodBelongs = await connection.ExecuteScalarAsync<bool>(
                    "SELECT COUNT(1) FROM foods WHERE id = @FoodId AND restaurant_id = @RestaurantId",
                    new { FoodId = foodId, RestaurantId = order.RestaurantId });

                if (!foodBelongs)
                {
                    throw new Exception($"Food with ID {foodId} does not belong to restaurant ID {order.RestaurantId}.");
                }

                await connection.ExecuteAsync(
                    "INSERT INTO order_items (order_id, food_id, quantity) VALUES (@OrderId, @FoodId, @Quantity)",
                    new { OrderId = orderId, FoodId = foodId, item.Quantity }, transaction);
            }

            await transaction.CommitAsync();
            return orderId;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<IEnumerable<OrderModel>> GetOrdersAsync(int restaurantId)
    {
        using var connection = new SQLiteConnection(_connectionString);
        var orderDictionary = new Dictionary<int, OrderModel>();

        var query = @"
            SELECT o.*, oi.order_id AS OrderId, oi.quantity, f.id AS FoodId, f.name, f.price, f.description 
            FROM orders o
            LEFT JOIN order_items oi ON o.id = oi.order_id
            LEFT JOIN foods f ON oi.food_id = f.id
            WHERE o.restaurant_id = @RestaurantId";

        var orders = await connection.QueryAsync<OrderModel, OrderItemModel, FoodModel, OrderModel>(query,
            (order, orderItem, food) =>
            {
                if (!orderDictionary.TryGetValue(order.Id, out var currentOrder))
                {
                    currentOrder = order;
                    orderDictionary.Add(currentOrder.Id, currentOrder);
                }

                if (orderItem != null)
                {
                    orderItem.Food = food;
                    currentOrder.Items.Add(orderItem);
                }

                return currentOrder;
            }, new { RestaurantId = restaurantId }, splitOn: "OrderId,FoodId");

        return orders.Distinct().ToList();
    }

    public async Task<OrderModel> GetOrderByIdAsync(int id, int restaurantId)
    {
        using var connection = new SQLiteConnection(_connectionString);
        var orderDictionary = new Dictionary<int, OrderModel>();

        var query = @"
            SELECT o.*, oi.order_id AS OrderId, oi.quantity, f.id AS FoodId, f.name, f.price, f.description 
            FROM orders o
            LEFT JOIN order_items oi ON o.id = oi.order_id
            LEFT JOIN foods f ON oi.food_id = f.id
            WHERE o.id = @Id AND o.restaurant_id = @RestaurantId";

        var orders = await connection.QueryAsync<OrderModel, OrderItemModel, FoodModel, OrderModel>(query,
            (order, orderItem, food) =>
            {
                if (!orderDictionary.TryGetValue(order.Id, out var currentOrder))
                {
                    currentOrder = order;
                    orderDictionary.Add(currentOrder.Id, currentOrder);
                }

                if (orderItem != null)
                {
                    orderItem.Food = food;
                    currentOrder.Items.Add(orderItem);
                }

                return currentOrder;
            }, new { Id = id, RestaurantId = restaurantId }, splitOn: "OrderId,FoodId");

        return orders.FirstOrDefault();
    }
}