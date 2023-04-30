using System;
namespace RonWeb.API.Interface.Shared
{
	public interface IDeleteAsync<T>
	{
        public Task DeleteAsync(string id);
    }
}

