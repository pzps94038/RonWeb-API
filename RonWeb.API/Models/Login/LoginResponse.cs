﻿using System;
using RonWeb.API.Models.Shared;

namespace RonWeb.API.Models.Login
{
	public class LoginResponse
	{
		/// <summary>
		/// Token
		/// </summary>
		public Token Token { get; set; } = new Token();
		public long UserId { get; set; }
	}
}

