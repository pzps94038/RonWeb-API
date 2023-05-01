using System;
namespace RonWeb.API.Models.CustomizeException
{
    public class NotFoundException: Exception
    {
        public NotFoundException(): base("找不到資源") { }
        public NotFoundException(string msg) : base(msg) { }
    }
}

