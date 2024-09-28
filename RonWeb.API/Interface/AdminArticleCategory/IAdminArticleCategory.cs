using System;
using RonWeb.API.Interface.Shared;
using RonWeb.API.Models.ArticleCategory;

namespace RonWeb.API.Interface.AdminArticleCategory
{
    public interface IAdminArticleCategoryHelper : IGetAsync<long, Category>,
        ICreateAsync<CreateArticleCategoryRequest>,
        IUpdateAsync<long, UpdateArticleCategoryRequest>,
        IDeleteAsync<long>
    {
        public Task<GetArticleCategoryResponse> GetListAsync(int? page);
    }
}

