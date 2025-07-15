using System;

namespace RonWeb.API.Interface.AdminCodeType
{
    public interface IAdminCodeTypeDelete
    {
        public Task DeleteAsync(long data);
    }
}