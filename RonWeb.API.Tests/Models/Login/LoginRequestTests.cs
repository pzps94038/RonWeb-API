using RonWeb.API.Models.Login;
using Xunit;

namespace RonWeb.API.Tests.Models.Login
{
    public class LoginRequestTests
    {
        [Fact]
        public void LoginRequest_DefaultConstructor_ShouldInitializeWithEmptyValues()
        {
            // Act
            var loginRequest = new LoginRequest();

            // Assert
            Assert.NotNull(loginRequest);
            Assert.Equal(string.Empty, loginRequest.Account);
            Assert.Equal(string.Empty, loginRequest.Password);
        }

        [Fact]
        public void LoginRequest_Properties_ShouldBeSetCorrectly()
        {
            // Arrange
            var account = "testuser";
            var password = "testpassword";

            // Act
            var loginRequest = new LoginRequest
            {
                Account = account,
                Password = password
            };

            // Assert
            Assert.Equal(account, loginRequest.Account);
            Assert.Equal(password, loginRequest.Password);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("user", "")]
        [InlineData("", "password")]
        [InlineData("user", "password")]
        [InlineData("test@example.com", "strongpassword123")]
        public void LoginRequest_WithVariousValues_ShouldSetPropertiesCorrectly(string account, string password)
        {
            // Act
            var loginRequest = new LoginRequest
            {
                Account = account,
                Password = password
            };

            // Assert
            Assert.Equal(account, loginRequest.Account);
            Assert.Equal(password, loginRequest.Password);
        }
    }
}