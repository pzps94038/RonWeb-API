using System;
using RonWeb.API.Models.ArticleCategory;

namespace RonWeb.API.Interface.AdminArticleCategory
{
    public interface IAdminArticleCategoryCreate
    {
        public Task CreateAsync(CreateArticleCategoryRequest data);
    }
}