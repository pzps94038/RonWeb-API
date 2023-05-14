using System;
using RonWeb.API.Models.Shared;

namespace RonWeb.API.Models.Login
{
	public class LoginResponse
	{
		/// <summary>
		/// Token
		/// </summary>
		public Token Token { get; set; } = new Token();
		public string UserId { get; set; } = string.Empty;
	}
}

