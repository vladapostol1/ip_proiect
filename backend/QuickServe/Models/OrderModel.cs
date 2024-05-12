namespace QuickServe.Models;

public class OrderModel
{
    public int Id { get; set;}
    public int RestaurantId{ get; set;}
    public int TableNumber{ get; set;}
    public double Total{ get; set;}
    public DateTime OrderTime {get; set;}
    public List<OrderItemModel> Items { get; set; } = new List<OrderItemModel>();
}
