using System;
using RonWeb.API.Models.Shared;

namespace RonWeb.API.Models.Login
{
	public class LoginResponse
	{
		public Token Token { get; set; } = new Token();
	}
}

