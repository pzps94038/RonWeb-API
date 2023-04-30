using System;
namespace RonWeb.API.Interface.Shared
{
	public interface IGetListAsync<T>
	{
        public Task<List<T>> GetListAsync(object? request);
    }
}

