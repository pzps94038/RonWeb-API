using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using RonWeb.Core;
using Xunit;

namespace RonWeb.Core.Tests
{
    public class JwtToolTests
    {
        [Fact]
        public void GenerateToken_WithValidJwtModel_ShouldReturnValidToken()
        {
            // Arrange
            var jwtModel = new JwtModel
            {
                Key = "ThisIsAVeryLongSecretKeyThatIsAtLeast32Characters",
                Issuer = "TestIssuer",
                Audience = "TestAudience",
                ExpirationTime = DateTime.UtcNow.AddHours(1),
                Claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, "test@example.com"),
                    new Claim(ClaimTypes.NameIdentifier, "123")
                }
            };

            // Act
            var token = JwtTool.GenerateToken(jwtModel);

            // Assert
            Assert.NotNull(token);
            Assert.NotEmpty(token);
            
            // Verify token structure
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwt = tokenHandler.ReadJwtToken(token);
            
            Assert.Equal(jwtModel.Issuer, jwt.Issuer);
            Assert.Equal(jwtModel.Audience, jwt.Audiences.First());
            Assert.Contains(jwt.Claims, c => c.Type == ClaimTypes.Email && c.Value == "test@example.com");
            Assert.Contains(jwt.Claims, c => c.Type == ClaimTypes.NameIdentifier && c.Value == "123");
        }

        [Fact]
        public void CreateClaims_WithValidParameters_ShouldReturnCorrectClaims()
        {
            // Arrange
            var email = "test@example.com";
            var userId = "123";
            var role = "Admin";

            // Act
            var claims = JwtTool.CreateClaims(email, userId, role);

            // Assert
            Assert.NotNull(claims);
            Assert.Equal(3, claims.Count);
            
            var emailClaim = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email);
            Assert.NotNull(emailClaim);
            Assert.Equal(email, emailClaim.Value);
            
            var userIdClaim = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.NameId);
            Assert.NotNull(userIdClaim);
            Assert.Equal(userId, userIdClaim.Value);
            
            var roleClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
            Assert.NotNull(roleClaim);
            Assert.Equal(role, roleClaim.Value);
        }

        [Fact]
        public void CreateRefreshToken_ShouldReturnBase64String()
        {
            // Act
            var refreshToken = JwtTool.CreateRefreshToken();

            // Assert
            Assert.NotNull(refreshToken);
            Assert.NotEmpty(refreshToken);
            
            // Verify it's a valid base64 string
            var bytes = Convert.FromBase64String(refreshToken);
            Assert.Equal(32, bytes.Length);
        }

        [Fact]
        public void CreateRefreshToken_ShouldReturnDifferentTokensOnMultipleCalls()
        {
            // Act
            var refreshToken1 = JwtTool.CreateRefreshToken();
            var refreshToken2 = JwtTool.CreateRefreshToken();

            // Assert
            Assert.NotEqual(refreshToken1, refreshToken2);
        }

        [Theory]
        [InlineData("", "TestIssuer", "TestAudience")]
        public void GenerateToken_WithEmptyKey_ShouldThrowArgumentException(string key, string issuer, string audience)
        {
            // Arrange
            var jwtModel = new JwtModel
            {
                Key = key,
                Issuer = issuer,
                Audience = audience,
                ExpirationTime = DateTime.UtcNow.AddHours(1),
                Claims = new List<Claim>()
            };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => JwtTool.GenerateToken(jwtModel));
        }

        [Theory]
        [InlineData("TooShort", "TestIssuer", "TestAudience")]
        public void GenerateToken_WithShortKey_ShouldThrowArgumentOutOfRangeException(string key, string issuer, string audience)
        {
            // Arrange
            var jwtModel = new JwtModel
            {
                Key = key,
                Issuer = issuer,
                Audience = audience,
                ExpirationTime = DateTime.UtcNow.AddHours(1),
                Claims = new List<Claim>()
            };

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => JwtTool.GenerateToken(jwtModel));
        }

        [Fact]
        public void JwtModel_Properties_ShouldBeSetCorrectly()
        {
            // Arrange
            var key = "TestKey";
            var issuer = "TestIssuer";
            var audience = "TestAudience";
            var expirationTime = DateTime.UtcNow.AddHours(1);
            var claims = new List<Claim> { new Claim(ClaimTypes.Email, "test@example.com") };

            // Act
            var jwtModel = new JwtModel
            {
                Key = key,
                Issuer = issuer,
                Audience = audience,
                ExpirationTime = expirationTime,
                Claims = claims
            };

            // Assert
            Assert.Equal(key, jwtModel.Key);
            Assert.Equal(issuer, jwtModel.Issuer);
            Assert.Equal(audience, jwtModel.Audience);
            Assert.Equal(expirationTime, jwtModel.ExpirationTime);
            Assert.Equal(claims, jwtModel.Claims);
        }

        [Fact]
        public void JwtModel_DefaultValues_ShouldBeCorrect()
        {
            // Act
            var jwtModel = new JwtModel();

            // Assert
            Assert.Equal(string.Empty, jwtModel.Key);
            Assert.Equal(string.Empty, jwtModel.Issuer);
            Assert.Equal(string.Empty, jwtModel.Audience);
            Assert.Equal(default(DateTime), jwtModel.ExpirationTime);
            Assert.NotNull(jwtModel.Claims);
            Assert.Empty(jwtModel.Claims);
        }
    }
}