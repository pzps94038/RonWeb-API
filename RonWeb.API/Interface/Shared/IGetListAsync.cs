using System;
using RonWeb.API.Enum;
using RonWeb.API.Models.Article;

namespace RonWeb.API.Interface.Shared
{
    public interface IGetListAsync<T, R>
    {
        public Task<List<T>> GetListAsync(R request);
    }

    public interface IGetListAsync<T>
    {
        public Task<List<T>> GetListAsync();
    }
}
