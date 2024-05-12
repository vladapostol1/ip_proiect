namespace QuickServe.Interfaces;

using QuickServe.Models;

public interface IOrderService
{
        Task<int> CreateOrderAsync(OrderModel order, List<OrderItemModel> items);
        Task<IEnumerable<OrderModel>> GetOrdersAsync(int restaurantId);
        Task<OrderModel> GetOrderByIdAsync(int id, int restaurantId);
}
