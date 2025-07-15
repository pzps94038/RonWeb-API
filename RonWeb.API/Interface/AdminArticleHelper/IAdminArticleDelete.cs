using System;

namespace RonWeb.API.Interface.AdminArticleHelper
{
    public interface IAdminArticleDelete
    {
        public Task DeleteAsync(long data);
    }
}