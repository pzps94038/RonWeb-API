using System;
namespace RonWeb.API.Interface.Shared
{
	public interface IUpdateAsync<T>
	{
        public Task UpdateAsync(string id, T data);
    }
}

