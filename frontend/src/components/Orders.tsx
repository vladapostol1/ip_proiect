import React, { useEffect, useState } from "react";
import axiosInstance from "../context/axiosInstance";

interface FoodModel {
  id: number;
  restaurantId: number;
  name: string;
  price: number;
  description: string;
  category: string;
}

interface OrderItemModel {
  orderId: number;
  quantity: number;
  food: FoodModel;
}

interface OrderModel {
  id: number;
  restaurantId: number;
  tableNumber: number;
  total: number;
  orderTime: string;
  items: OrderItemModel[];
}

const Orders: React.FC = () => {
  const [orders, setOrders] = useState<OrderModel[]>([]);
  const [loading, setLoading] = useState<boolean>(true);

  useEffect(() => {
    const fetchOrders = async () => {
      try {
        const response = await axiosInstance.get<OrderModel[]>("/orders");
        setOrders(response.data);
        setLoading(false);
      } catch (error) {
        console.error("Failed to fetch orders:", error);
        setLoading(false);
      }
    };

    fetchOrders();
  }, []);

  if (loading) {
    return <div>Loading...</div>;
  }

  return (
    <div className="p-4">
      <h2 className="text-2xl font-bold mb-4">Orders</h2>
      {orders.length === 0 ? (
        <p>No orders found</p>
      ) : (
        <div className="space-y-4">
          {orders.map((order) => (
            <div key={order.id} className="border p-4 rounded shadow">
              <h3 className="text-xl font-bold">Order #{order.id}</h3>
              <p>Table Number: {order.tableNumber}</p>
              <p>Total: ${order.total.toFixed(2)}</p>
              <p>Order Time: {new Date(order.orderTime).toLocaleString()}</p>
              <div className="mt-4">
                <h4 className="text-lg font-bold">Items:</h4>
                <ul className="list-disc list-inside">
                  {order.items.map((item) => (
                    <li key={item.food.id}>
                      {item.food.name} - Quantity: {item.quantity} - Price: $
                      {item.food.price}
                    </li>
                  ))}
                </ul>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default Orders;
