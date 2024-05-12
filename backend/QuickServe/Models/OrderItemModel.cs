namespace QuickServe.Models;

public class OrderItemModel
{
    public int OrderId { get; set; }
    public int Quantity{ get; set; }
    public FoodModel Food{ get; set; }

}
