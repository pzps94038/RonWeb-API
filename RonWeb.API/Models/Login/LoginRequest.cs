using System;
namespace RonWeb.API.Models.Login
{
	public class LoginRequest
	{
		public string Account { get; set; } = string.Empty;
		public string Password { get; set; } = string.Empty;
    }
}

