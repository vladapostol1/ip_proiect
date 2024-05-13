import React, { useState } from "react";
import axiosInstance from "../context/axiosInstance";

interface FoodModel {
  id: number;
  restaurantId: number;
  name: string;
  price: number;
  description: string;
  category: string;
}

const AddFood: React.FC = () => {
  const [food, setFood] = useState<Partial<FoodModel>>({
    name: "",
    price: 0,
    description: "",
    category: "",
  });

  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
  ) => {
    const { name, value } = e.target;
    setFood((prevFood) => ({
      ...prevFood,
      [name]: value,
    }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await axiosInstance.post("/foods", food);
      alert("Food added successfully.");
      setFood({
        name: "",
        price: 0,
        description: "",
        category: "",
      });
    } catch (error) {
      console.error("Failed to add food:", error);
      alert("Failed to add food.");
    }
  };

  return (
    <form onSubmit={handleSubmit} className="p-4">
      <div className="mb-4">
        <label className="block text-gray-700">Name</label>
        <input
          type="text"
          name="name"
          value={food.name || ""}
          onChange={handleChange}
          className="w-full p-2 border border-gray-300 rounded"
          required
        />
      </div>
      <div className="mb-4">
        <label className="block text-gray-700">Price</label>
        <input
          type="number"
          name="price"
          value={food.price || 0}
          onChange={handleChange}
          className="w-full p-2 border border-gray-300 rounded"
          required
        />
      </div>
      <div className="mb-4">
        <label className="block text-gray-700">Description</label>
        <textarea
          name="description"
          value={food.description || ""}
          onChange={handleChange}
          className="w-full p-2 border border-gray-300 rounded"
          required
        />
      </div>
      <div className="mb-4">
        <label className="block text-gray-700">Category</label>
        <input
          type="text"
          name="category"
          value={food.category || ""}
          onChange={handleChange}
          className="w-full p-2 border border-gray-300 rounded"
          required
        />
      </div>
      <button
        type="submit"
        className="px-4 py-2 bg-blue-600 text-white rounded"
      >
        Add Food
      </button>
    </form>
  );
};

export default AddFood;
