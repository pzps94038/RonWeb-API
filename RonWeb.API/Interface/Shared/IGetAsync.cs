using System;
namespace RonWeb.API.Interface.Shared
{
	public interface IGetAsync<T>
	{
		public Task<T> GetAsync();
	}
}

