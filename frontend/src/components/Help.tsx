import React from "react";

const Help: React.FC = () => {
  return (
    <div className="p-6 text-gray-200 rounded-lg shadow-lg h-screen overflow-auto">
      <h1 className="text-2xl font-bold mb-4">Help & Documentation</h1>
      
      <section className="mb-6">
        <h2 className="text-xl font-semibold mb-2">Dashboard Overview</h2>
        <p>
          The dashboard is the main interface of the application, where you can navigate between different sections using the sidebar. Each section provides specific functionalities to manage your restaurant operations.
        </p>
      </section>
      
      <section className="mb-6">
        <h2 className="text-xl font-semibold mb-2">Menu Management</h2>
        <p>
          In the <strong>Menu</strong> section, you can view all the food items available in your restaurant. The items are categorized, and you can filter them by category using the buttons provided at the top.
        </p>
        <ul className="list-disc list-inside">
          <li><strong>View Food Items:</strong> Browse through all the available food items along with their details such as name, price, description, and category.</li>
        </ul>
      </section>
      
      <section className="mb-6">
        <h2 className="text-xl font-semibold mb-2">Order Management</h2>
        <p>
          The <strong>Orders</strong> section allows you to manage all the orders placed in your restaurant. You can view detailed information about each order, including the table number, total amount, order time, and items ordered.
        </p>
        <ul className="list-disc list-inside">
          <li><strong>View Orders:</strong> See a list of all orders with detailed information about each order.</li>
        </ul>
      </section>
      
      <section className="mb-6">
        <h2 className="text-xl font-semibold mb-2">Creating Orders</h2>
        <p>
          In the <strong>Create Order</strong> section, you can create new orders by selecting food items from the menu. Specify the table number and the quantity of each item ordered.
        </p>
        <ul className="list-disc list-inside">
          <li><strong>Add Items:</strong> Select food items to add to the order.</li>
          <li><strong>Remove Items:</strong> Remove items from the order if necessary.</li>
          <li><strong>Adjust Quantity:</strong> Change the quantity of each food item in the order.</li>
          <li><strong>Submit Order:</strong> Submit the order once all items and quantities are finalized.</li>
        </ul>
      </section>
      
      <section className="mb-6">
        <h2 className="text-xl font-semibold mb-2">Adding New Food</h2>
        <p>
          The <strong>New Food</strong> section allows you to add new food items to your restaurant’s menu. Fill in the details such as name, price, description, and category.
        </p>
        <ul className="list-disc list-inside">
          <li><strong>Name:</strong> Enter the name of the food item.</li>
          <li><strong>Price:</strong> Specify the price of the food item.</li>
          <li><strong>Description:</strong> Provide a description of the food item.</li>
          <li><strong>Category:</strong> Assign a category to the food item for easier filtering and organization.</li>
          <li><strong>Submit:</strong> Add the food item to the menu by submitting the form.</li>
        </ul>
      </section>
      
      <section className="mb-6">
        <h2 className="text-xl font-semibold mb-2">User Authentication</h2>
        <p>
          The application requires user authentication for accessing most functionalities. You can register a new user, log in with existing credentials, and log out when finished.
        </p>
        <ul className="list-disc list-inside">
          <li><strong>Register:</strong> Create a new user account.</li>
          <li><strong>Login:</strong> Log in with your username and password.</li>
          <li><strong>Logout:</strong> Log out of the application.</li>
        </ul>
      </section>

      <section className="mb-6">
        <h2 className="text-xl font-semibold mb-2">Navigation</h2>
        <p>
          Use the sidebar to navigate between different sections of the application:
        </p>
        <ul className="list-disc list-inside">
          <li><strong>Menu:</strong> View and manage food items.</li>
          <li><strong>Orders:</strong> View and manage orders.</li>
          <li><strong>Create Order:</strong> Create new orders.</li>
          <li><strong>New Food:</strong> Add new food items to the menu.</li>
        </ul>
      </section>
    </div>
  );
};

export default Help;
