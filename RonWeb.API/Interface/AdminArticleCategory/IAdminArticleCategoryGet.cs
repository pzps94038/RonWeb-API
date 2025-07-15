using System;
using RonWeb.API.Models.ArticleCategory;

namespace RonWeb.API.Interface.AdminArticleCategory
{
    public interface IAdminArticleCategoryGet
    {
        public Task<Category> GetAsync(long id);
    }
}