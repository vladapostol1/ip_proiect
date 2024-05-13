import React, { useState, useEffect } from "react";
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

const MakeOrder: React.FC = () => {
  const [foods, setFoods] = useState<FoodModel[]>([]);
  const [orderItems, setOrderItems] = useState<OrderItemModel[]>([]);
  const [tableNumber, setTableNumber] = useState<number>(0);

  useEffect(() => {
    const fetchFoods = async () => {
      try {
        const response = await axiosInstance.get<FoodModel[]>("/foods");
        setFoods(response.data);
      } catch (error) {
        console.error("Failed to fetch foods:", error);
      }
    };

    fetchFoods();
  }, []);

  const handleAddItem = (food: FoodModel) => {
    const existingItem = orderItems.find((item) => item.food.id === food.id);
    if (existingItem) {
      setOrderItems(
        orderItems.map((item) =>
          item.food.id === food.id
            ? { ...item, quantity: item.quantity + 1 }
            : item
        )
      );
    } else {
      setOrderItems([...orderItems, { orderId: 0, quantity: 1, food }]);
    }
  };

  const handleRemoveItem = (foodId: number) => {
    setOrderItems(orderItems.filter((item) => item.food.id !== foodId));
  };

  const handleQuantityChange = (foodId: number, quantity: number) => {
    setOrderItems(
      orderItems.map((item) =>
        item.food.id === foodId ? { ...item, quantity } : item
      )
    );
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    const order: OrderModel = {
      id: 0,
      restaurantId: 0,
      tableNumber,
      total: orderItems.reduce(
        (acc, item) => acc + item.food.price * item.quantity,
        0
      ),
      orderTime: new Date().toISOString(),
      items: orderItems,
    };

    try {
      await axiosInstance.post("/orders", order);
      alert("Order created successfully.");
      setTableNumber(0);
      setOrderItems([]);
    } catch (error) {
      console.error("Failed to create order:", error);
      alert("Failed to create order.");
    }
  };

  return (
    <form onSubmit={handleSubmit} className="p-4">
      <div className="mb-4">
        <label className="block text-gray-700">Table Number</label>
        <input
          type="number"
          value={tableNumber}
          onChange={(e) => setTableNumber(parseInt(e.target.value, 10))}
          className="w-full p-2 border border-gray-300 rounded"
          required
        />
      </div>
      <div className="mb-4">
        <label className="block text-gray-700">Select Food Items</label>
        {foods.map((food) => (
          <div key={food.id} className="flex items-center mb-2">
            <span className="mr-2">
              {food.name} - ${food.price}
            </span>
            <button
              type="button"
              onClick={() => handleAddItem(food)}
              className="px-2 py-1 bg-blue-600 text-white rounded"
            >
              Add
            </button>
          </div>
        ))}
      </div>
      <div className="mb-4">
        <label className="block text-gray-700">Order Items</label>
        {orderItems.length === 0 ? (
          <p>No items added</p>
        ) : (
          orderItems.map((item) => (
            <div key={item.food.id} className="flex items-center mb-2">
              <span className="mr-2">{item.food.name}</span>
              <input
                type="number"
                value={item.quantity}
                onChange={(e) =>
                  handleQuantityChange(
                    item.food.id,
                    parseInt(e.target.value, 10)
                  )
                }
                className="w-16 p-2 border border-gray-300 rounded mr-2"
                min="1"
              />
              <button
                type="button"
                onClick={() => handleRemoveItem(item.food.id)}
                className="px-2 py-1 bg-red-600 text-white rounded"
              >
                Remove
              </button>
            </div>
          ))
        )}
      </div>
      <button
        type="submit"
        className="px-4 py-2 bg-blue-600 text-white rounded"
      >
        Create Order
      </button>
    </form>
  );
};

export default MakeOrder;
