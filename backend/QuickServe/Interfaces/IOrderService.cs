namespace QuickServe.Interfaces;

using QuickServe.Models;

public interface IOrderService
{
    Task<int> CreateOrderAsync(OrderModel order, List<OrderItemModel> items);
    Task<IEnumerable<OrderModel>> GetOrdersAsync();
    Task<OrderModel> GetOrderByIdAsync(int id);
}
