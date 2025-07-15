using System;

namespace RonWeb.API.Interface.AdminCode
{
    public interface IAdminCodeDelete
    {
        public Task DeleteAsync(long data);
    }
}