using System;
using RonWeb.API.Models.ArticleCategory;

namespace RonWeb.API.Interface.AdminArticleCategory
{
    public interface IAdminArticleCategoryHelper
    {
        public Task<GetArticleCategoryResponse> GetListAsync(int? page);
        public Task<Category> GetAsync(long id);
        public Task CreateAsync(CreateArticleCategoryRequest data);
        public Task UpdateAsync(long id, UpdateArticleCategoryRequest data);
        public Task DeleteAsync(long data);
    }
}

