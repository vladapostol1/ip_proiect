using Moq;
using Xunit;
using QuickServe.Interfaces;
using QuickServe.Models;
using QuickServe.Services;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.SQLite;
using Dapper;

namespace QuickServe.Tests
{
    public class FoodServiceTests
    {
        private readonly FoodService _foodService;
        private readonly string _connectionString = "Data Source=:memory:";

        public FoodServiceTests()
        {
            var mockConfig = new Mock<IConfiguration>();
            var mockSection = new Mock<IConfigurationSection>();
            mockSection.Setup(x => x.Value).Returns(_connectionString);
            mockConfig.Setup(x => x.GetSection("ConnectionStrings:SQLiteConnection")).Returns(mockSection.Object);

            _foodService = new FoodService(mockConfig.Object);
        }

        private async Task InitializeDatabase()
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                var createTableQuery = @"
                    CREATE TABLE foods (
                        id INTEGER PRIMARY KEY,
                        restaurant_id INTEGER,
                        name TEXT,
                        price REAL,
                        description TEXT,
                        category TEXT
                    );";
                await connection.ExecuteAsync(createTableQuery);
            }
        }

        [Fact]
        public async Task GetFoodsAsync_ShouldReturnFoods_ForGivenRestaurantId()
        {
            // Arrange
            await InitializeDatabase();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                var insertQuery = "INSERT INTO foods (restaurant_id, name, price, description, category) VALUES (@RestaurantId, @Name, @Price, @Description, @Category)";
                await connection.ExecuteAsync(insertQuery, new { RestaurantId = 1, Name = "Pizza", Price = 9.99, Description = "Cheese Pizza", Category = "Italian" });
            }

            // Act
            var result = await _foodService.GetFoodsAsync(1);

            // Assert
            Assert.Single(result);
        }

        [Fact]
        public async Task AddFoodAsync_ShouldAddFood()
        {
            // Arrange
            await InitializeDatabase();
            var food = new FoodModel
            {
                RestaurantId = 1,
                Name = "Burger",
                Price = 5.99,
                Description = "Beef Burger",
                Category = "American"
            };

            // Act
            await _foodService.AddFoodAsync(food);
            var result = await _foodService.GetFoodsAsync(1);

            // Assert
            Assert.Single(result);
            Assert.Equal("Burger", result.First().Name);
        }

        [Fact]
        public async Task GetFoodByIdAsync_ShouldReturnFood()
        {
            // Arrange
            await InitializeDatabase();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                var insertQuery = "INSERT INTO foods (restaurant_id, name, price, description, category) VALUES (@RestaurantId, @Name, @Price, @Description, @Category)";
                await connection.ExecuteAsync(insertQuery, new { RestaurantId = 1, Name = "Pizza", Price = 9.99, Description = "Cheese Pizza", Category = "Italian" });
            }

            // Act
            var result = await _foodService.GetFoodByIdAsync(1, 1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Pizza", result.Name);
        }
    }
}
