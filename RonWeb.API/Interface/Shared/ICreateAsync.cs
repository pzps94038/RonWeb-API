using System;
namespace RonWeb.API.Interface.Shared
{
	public interface ICreateAsync<T>
	{
        public Task CreateAsync(T data);
    }
}

