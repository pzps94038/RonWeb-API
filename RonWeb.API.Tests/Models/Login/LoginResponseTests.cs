using RonWeb.API.Models.Login;
using RonWeb.API.Models.Shared;
using Xunit;

namespace RonWeb.API.Tests.Models.Login
{
    public class LoginResponseTests
    {
        [Fact]
        public void LoginResponse_DefaultConstructor_ShouldInitializeWithDefaultValues()
        {
            // Act
            var loginResponse = new LoginResponse();

            // Assert
            Assert.NotNull(loginResponse);
            Assert.NotNull(loginResponse.Token);
            Assert.Equal(0, loginResponse.UserId);
        }

        [Fact]
        public void LoginResponse_Properties_ShouldBeSetCorrectly()
        {
            // Arrange
            var token = new Token("access_token", "refresh_token");
            var userId = 123L;

            // Act
            var loginResponse = new LoginResponse
            {
                Token = token,
                UserId = userId
            };

            // Assert
            Assert.Equal(token, loginResponse.Token);
            Assert.Equal(userId, loginResponse.UserId);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(999)]
        [InlineData(long.MaxValue)]
        public void LoginResponse_WithVariousUserIds_ShouldSetUserIdCorrectly(long userId)
        {
            // Act
            var loginResponse = new LoginResponse
            {
                UserId = userId
            };

            // Assert
            Assert.Equal(userId, loginResponse.UserId);
        }

        [Fact]
        public void LoginResponse_TokenProperty_ShouldBeAccessibleAndMutable()
        {
            // Arrange
            var loginResponse = new LoginResponse();
            var newToken = new Token("new_access", "new_refresh");

            // Act
            loginResponse.Token = newToken;

            // Assert
            Assert.Equal(newToken, loginResponse.Token);
            Assert.Equal("new_access", loginResponse.Token.AccessToken);
            Assert.Equal("new_refresh", loginResponse.Token.RefreshToken);
        }
    }
}