using System;
namespace RonWeb.API.Interface.Shared
{
	public interface IGetByIdAsync<T>
	{
        public Task<T> GetByIdAsync(string id);
    }
}

