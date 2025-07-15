using System;

namespace RonWeb.API.Interface.AdminArticleCategory
{
    public interface IAdminArticleCategoryDelete
    {
        public Task DeleteAsync(long data);
    }
}