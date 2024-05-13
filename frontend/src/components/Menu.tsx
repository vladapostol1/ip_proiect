import React, { useState, useEffect } from "react";
import axiosInstance from "../context/axiosInstance";
import FoodItem from "./FoodItem";

interface FoodModel {
  id: number;
  restaurantId: number;
  name: string;
  price: number;
  description: string;
  category: string;
}

const Menu: React.FC = () => {
  const [foods, setFoods] = useState<FoodModel[]>([]);
  const [categories, setCategories] = useState<string[]>(["All"]);
  const [selectedCategory, setSelectedCategory] = useState<string>("All");

  useEffect(() => {
    const fetchFoods = async () => {
      try {
        const response = await axiosInstance.get<FoodModel[]>("/foods");
        setFoods(response.data);

        const uniqueCategories: string[] = [
          "All",
          ...new Set(response.data.map((food) => food.category)),
        ];
        setCategories(uniqueCategories);
      } catch (error) {
        console.error("Failed to fetch foods:", error);
      }
    };

    fetchFoods();
  }, []);

  const filteredFoods =
    selectedCategory === "All"
      ? foods
      : foods.filter((food) => food.category === selectedCategory);

  return (
    <div className="flex-grow p-4">
      <div className="flex space-x-4 mb-4">
        {categories.map((category) => (
          <button
            key={category}
            onClick={() => setSelectedCategory(category)}
            className={`px-4 py-2 rounded ${
              selectedCategory === category
                ? "bg-blue-600 text-white"
                : "bg-gray-800 text-gray-400"
            }`}
          >
            {category}
          </button>
        ))}
      </div>
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
        {filteredFoods.length > 0
          ? filteredFoods.map((food) => (
              <FoodItem
                key={food.id}
                id={food.id}
                name={food.name}
                price={food.price}
                description={food.description}
              />
            ))
          : "No food was found"}
      </div>
    </div>
  );
};

export default Menu;
