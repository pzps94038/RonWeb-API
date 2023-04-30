using System;
namespace RonWeb.API.Interface.Shared
{
	public interface IGetListAsync<T, R>
	{
        public Task<List<T>> GetListAsync(R request);
    }
}

