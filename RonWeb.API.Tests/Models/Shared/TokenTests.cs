using RonWeb.API.Models.Shared;
using Xunit;

namespace RonWeb.API.Tests.Models.Shared
{
    public class TokenTests
    {
        [Fact]
        public void Token_DefaultConstructor_ShouldInitializeWithEmptyValues()
        {
            // Act
            var token = new Token();

            // Assert
            Assert.NotNull(token);
            Assert.Equal(string.Empty, token.AccessToken);
            Assert.Equal(string.Empty, token.RefreshToken);
        }

        [Fact]
        public void Token_ParameterizedConstructor_ShouldInitializeWithGivenValues()
        {
            // Arrange
            var accessToken = "test_access_token";
            var refreshToken = "test_refresh_token";

            // Act
            var token = new Token(accessToken, refreshToken);

            // Assert
            Assert.Equal(accessToken, token.AccessToken);
            Assert.Equal(refreshToken, token.RefreshToken);
        }

        [Fact]
        public void Token_Properties_ShouldBeSetCorrectly()
        {
            // Arrange
            var accessToken = "access_token_value";
            var refreshToken = "refresh_token_value";

            // Act
            var token = new Token
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

            // Assert
            Assert.Equal(accessToken, token.AccessToken);
            Assert.Equal(refreshToken, token.RefreshToken);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("access", "")]
        [InlineData("", "refresh")]
        [InlineData("access", "refresh")]
        [InlineData("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9", "refresh_token_123")]
        public void Token_WithVariousValues_ShouldSetPropertiesCorrectly(string accessToken, string refreshToken)
        {
            // Act
            var token = new Token(accessToken, refreshToken);

            // Assert
            Assert.Equal(accessToken, token.AccessToken);
            Assert.Equal(refreshToken, token.RefreshToken);
        }

        [Fact]
        public void Token_Properties_ShouldBeMutable()
        {
            // Arrange
            var token = new Token("initial_access", "initial_refresh");

            // Act
            token.AccessToken = "new_access";
            token.RefreshToken = "new_refresh";

            // Assert
            Assert.Equal("new_access", token.AccessToken);
            Assert.Equal("new_refresh", token.RefreshToken);
        }
    }
}