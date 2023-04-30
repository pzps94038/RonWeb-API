using System;
namespace RonWeb.API.Interface.Shared
{
	public interface IUpdateAsync<T> where T : new()
	{
        public Task UpdateAsync(string id, T data);
    }
}

