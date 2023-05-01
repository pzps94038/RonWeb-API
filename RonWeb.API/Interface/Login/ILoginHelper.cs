﻿using System;
using RonWeb.API.Models.Login;
using RonWeb.API.Models.Shared;

namespace RonWeb.API.Interface.Login
{
	public interface ILoginHelper
	{
		public Task<Token> Login(LoginRequest data);
	}
}

