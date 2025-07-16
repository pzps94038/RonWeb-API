using System;
namespace RonWeb.API.Interface.Shared
{
    public interface IGetListByRequestAsync<T, R>
    {
        public Task<List<T>> GetListAsync(R request);
    }
}