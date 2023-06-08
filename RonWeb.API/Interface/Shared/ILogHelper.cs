using System;
namespace RonWeb.API.Interface.Shared
{
	public interface ILogHelper
    {
        public void Warn(string msg);
        public void Error(Exception ex);
    }
}

