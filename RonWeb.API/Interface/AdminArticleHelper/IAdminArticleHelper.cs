using System;
using RonWeb.API.Models.Article;

namespace RonWeb.API.Interface.AdminArticleHelper
{
    public interface IAdminArticleHelper : IAdminArticleGet,
        IAdminArticleDelete,
        IAdminArticleUpdate,
        IAdminArticleCreate
    {
        public Task<GetArticleResponse> GetListAsync(int? page, string? keyword);
    }
}

