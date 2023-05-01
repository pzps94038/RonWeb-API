using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace RonWeb.Core
{
    public class JwtModel
    {
        public string Key { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public DateTime ExpirationTime { get; set; }
        public List<Claim> Claims { get; set; } = new List<Claim>();
    }

	public class JwtTool
	{
        /// <summary>
        /// 產生Token
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        public static string GenerateToken(JwtModel data)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(data.Key));
            var jwt = new JwtSecurityToken(
                    claims: data.Claims,
                    issuer: data.Issuer,
                    audience: data.Audience,
                    expires: data.ExpirationTime,
                    signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
                );
            var token = new JwtSecurityTokenHandler().WriteToken(jwt);
            return token;
        }

        /// <summary>
        /// 建立聲明
        /// </summary>
        /// <param name="email"></param>
        /// <param name="userId"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public static List<Claim> CreateClaims(string email, string userId, string role)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.NameId, userId),
                new Claim(ClaimTypes.Role, role)
            };
            return claims;
        }

        /// <summary>
        /// 產生亂數RefreshToken
        /// </summary>
        /// <returns></returns>
        public static string CreateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}

