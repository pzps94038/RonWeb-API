using System;
namespace RonWeb.API.Interface.Shared
{
    public interface IGetByIdAsync<T, R>
    {
        public Task<R> GetAsync(T id);
    }
}