using System;
using RonWeb.API.Models.ArticleCategory;

namespace RonWeb.API.Interface.AdminArticleCategory
{
    public interface IAdminArticleCategoryUpdate
    {
        public Task UpdateAsync(long id, UpdateArticleCategoryRequest data);
    }
}