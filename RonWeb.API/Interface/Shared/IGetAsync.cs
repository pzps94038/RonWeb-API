using System;
namespace RonWeb.API.Interface.Shared
{
	public interface IGetAsync<T>
	{
		public Task<T> GetAsync();
    }

    public interface IGetAsync<T, R>
    {
        public Task<R> GetAsync(T id);
    }
}

