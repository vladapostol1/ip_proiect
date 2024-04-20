namespace QuickServe.Services;

using System.Data.SQLite;
using Microsoft.Extensions.Configuration;
using QuickServe.Interfaces;

public class DatabaseService : IDatabaseService
{
    private readonly string _connectionString;

    public DatabaseService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("SQLiteConnection");
    }

    public string ConnectToDatabase()
    {
        using (var connection = new SQLiteConnection(_connectionString))
        {
            try
            {
                connection.Open();
                return "Connected to SQLite database successfully.";
            }
            catch (SQLiteException ex)
            {
                return "An error occurred: " + ex.Message;
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
