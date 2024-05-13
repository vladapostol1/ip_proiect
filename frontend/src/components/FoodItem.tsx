import React from "react";

interface FoodItemProps {
  id: number;
  name: string;
  price: number;
  description: string;
}

const FoodItem: React.FC<FoodItemProps> = ({
  id,
  name,
  price,
  description,
}) => {
  return (
    <div className="p-4 bg-zinc-800 rounded-lg shadow-md">
      <h3 className="text-xl font-semibold">{name}</h3>
      <p className="text-gray-400">{description}</p>
      <p className="text-lg font-bold">${price.toFixed(2)}</p>
    </div>
  );
};

export default FoodItem;
