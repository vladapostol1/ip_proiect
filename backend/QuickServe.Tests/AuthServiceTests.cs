using Xunit;
using QuickServe.Interfaces;
using QuickServe.Services;
using QuickServe.Contracts;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace QuickServe.Tests
{
    public class AuthServiceTests
    {
        private readonly IConfiguration _configuration;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            // Build configuration from appsettings.json
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            _configuration = configurationBuilder.Build();
            _authService = new AuthService(_configuration);
        }

        [Fact]
        public async Task RegisterUser_ShouldReturnTrue_WhenRegistrationIsSuccessful()
        {
            // Arrange
            var registrationContract = new RegistrationContract
            {
                Username = "testuser",
                Password = "password",
                RestaurantName = "Test Restaurant",
                RestaurantAddress = "123 Test St",
                RestaurantWebsite = "www.test.com"
            };

            // Act
            var result = await _authService.RegisterUser(registrationContract);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task AuthenticateUser_ShouldReturnUserModel_WhenCredentialsAreValid()
        {
            // Arrange
            var username = "testuser";
            var password = "password";
            await _authService.RegisterUser(new RegistrationContract
            {
                Username = username,
                Password = password,
                RestaurantName = "Test Restaurant",
                RestaurantAddress = "123 Test St",
                RestaurantWebsite = "www.test.com"
            });

            // Act
            var result = await _authService.AuthenticateUser(username, password);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(username, result.Username);
        }

        [Fact]
        public async Task AuthenticateUser_ShouldReturnNull_WhenCredentialsAreInvalid()
        {
            // Arrange
            var username = "wronguser";
            var password = "wrongpassword";

            // Act
            var result = await _authService.AuthenticateUser(username, password);

            // Assert
            Assert.Null(result);
        }
    }
}
