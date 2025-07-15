using System;
using RonWeb.API.Models.ArticleCategory;

namespace RonWeb.API.Interface.AdminArticleCategory
{
    public interface IAdminArticleCategoryHelper : IAdminArticleCategoryGet,
        IAdminArticleCategoryCreate,
        IAdminArticleCategoryUpdate,
        IAdminArticleCategoryDelete
    {
        public Task<GetArticleCategoryResponse> GetListAsync(int? page);
    }
}

