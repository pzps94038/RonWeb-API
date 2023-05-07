using System;
namespace RonWeb.API.Models.Register
{
	public class RegisterRequest
	{
		public string Account { get; set; } = string.Empty;
		public string Password { get; set; } = string.Empty;
		public string UserName { get; set; } = string.Empty;
		public string? Email { get; set; }
	}
}

