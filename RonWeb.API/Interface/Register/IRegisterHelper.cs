using System;
using RonWeb.API.Helper.Register;
using RonWeb.API.Models.Register;

namespace RonWeb.API.Interface.Register
{
	public interface IRegisterHelper
	{
        public Task RegisterUser(RegisterRequest data);
    }
}

