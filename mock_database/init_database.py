import sqlite3

connection = sqlite3.connect('restaurant_management.db')

cursor = connection.cursor()

cursor.execute("""
CREATE TABLE IF NOT EXISTS users (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    username TEXT NOT NULL,
    password TEXT NOT NULL,
    restaurant_id INTEGER,
    FOREIGN KEY (restaurant_id) REFERENCES restaurants (id)
);
""")

cursor.execute("""
CREATE TABLE IF NOT EXISTS restaurants (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    name TEXT NOT NULL,
    address TEXT,
    website TEXT
);
""")

cursor.execute("""
CREATE TABLE IF NOT EXISTS foods (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    name TEXT NOT NULL,
    price REAL NOT NULL,
    description TEXT,
    restaurant_id INTEGER,
    FOREIGN KEY (restaurant_id) REFERENCES restaurants (id)
);
""")

cursor.execute("""
CREATE TABLE IF NOT EXISTS orders (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    restaurant_id INTEGER,
    table_number INTEGER,
    total REAL,
    order_time TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (restaurant_id) REFERENCES restaurants (id)
);
""")

cursor.execute("""
CREATE TABLE IF NOT EXISTS order_items (
    order_id INTEGER,
    food_id INTEGER,
    quantity INTEGER,
    FOREIGN KEY (order_id) REFERENCES orders (id),
    FOREIGN KEY (food_id) REFERENCES foods (id),
    PRIMARY KEY (order_id, food_id)
);
""")

connection.commit()
connection.close()

print("Database-ul si tabelele au fost create cu success!")
