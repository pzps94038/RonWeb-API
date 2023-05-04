using System;
namespace RonWeb.API.Models.CustomizeException
{
	public class AuthFailException : Exception
    {
        public AuthFailException() : base("身分驗證失敗") { }
        public AuthFailException(string msg) : base(msg) { }
    }
}

