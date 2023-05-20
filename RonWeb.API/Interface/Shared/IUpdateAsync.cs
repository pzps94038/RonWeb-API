using System;
namespace RonWeb.API.Interface.Shared
{
	public interface IUpdateAsync<T, R>
	{
        public Task UpdateAsync(T id, R data);
    }
}

