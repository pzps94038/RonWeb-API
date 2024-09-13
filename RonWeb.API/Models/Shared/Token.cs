using System;
namespace RonWeb.API.Models.Shared
{
	public class Token
	{
        /// <summary>
        /// 驗證Token
        /// </summary>
        public string AccessToken { get; set; } = string.Empty;
        /// <summary>
        /// 刷新Token
        /// </summary>
        public string RefreshToken { get; set; } = string.Empty;
        public Token() { }
        public Token(string accessToken, string refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
    }
}

