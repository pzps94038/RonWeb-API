using System;
namespace RonWeb.API.Models.CustomizeException
{
	public class UniqueException: Exception
    {
        public UniqueException() : base("已有重複資料") { }
        public UniqueException(string msg) : base(msg) { }
    }
}

