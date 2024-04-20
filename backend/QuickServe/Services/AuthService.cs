namespace QuickServe.Services;

using QuickServe.Interfaces;
using QuickServe.Models;
using QuickServe.Utils;
using System.Data.SQLite;
using Microsoft.Extensions.Configuration;
using QuickServe.Contracts;

public class AuthService : IAuthService
{
    private readonly string _connectionString;

    public AuthService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("SQLiteConnection");
    }

public async Task<bool> RegisterUser(RegistrationContract contract)
{
    using (var connection = new SQLiteConnection(_connectionString))
    {
        connection.Open();
        using (var transaction = connection.BeginTransaction())
        {
            var restaurantCmd = new SQLiteCommand("INSERT INTO restaurants (name, address, website) VALUES (@name, @address, @website); SELECT last_insert_rowid();", connection, transaction);
            restaurantCmd.Parameters.AddWithValue("@name", contract.RestaurantName);
            restaurantCmd.Parameters.AddWithValue("@address", contract.RestaurantAddress);
            restaurantCmd.Parameters.AddWithValue("@website", contract.RestaurantWebsite);

            var restaurantId = (long)restaurantCmd.ExecuteScalar();

            var userCmd = new SQLiteCommand("INSERT INTO users (username, password, restaurant_id) VALUES (@username, @password, @restaurant_id)", connection, transaction);
            userCmd.Parameters.AddWithValue("@username", contract.Username);
            userCmd.Parameters.AddWithValue("@password", PasswordHasher.HashPassword(contract.Password)); // Hash the password before storing
            userCmd.Parameters.AddWithValue("@restaurant_id", restaurantId);

            var result = await userCmd.ExecuteNonQueryAsync();

            transaction.Commit();
            return result > 0;
        }
    }
}

    public async Task<UserModel> AuthenticateUser(string username, string password)
    {
        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();
            var cmd = new SQLiteCommand("SELECT id, username, password, restaurant_id FROM users WHERE username = @username", connection);
            cmd.Parameters.AddWithValue("@username", username);
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                if (reader.Read())
                {
                    var hashedPassword = reader["password"].ToString();
                    if (PasswordHasher.VerifyPassword(hashedPassword, password))
                    {
                        return new UserModel
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Username = reader["username"].ToString(),
                            RestaurantId = Convert.ToInt32(reader["restaurant_id"])
                        };
                    }
                }
            }
        }
        return null;
    }
}

