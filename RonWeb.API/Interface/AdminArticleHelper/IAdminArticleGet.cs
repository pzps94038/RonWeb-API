using System;
using RonWeb.API.Models.Article;

namespace RonWeb.API.Interface.AdminArticleHelper
{
    public interface IAdminArticleGet
    {
        public Task<GetByIdArticleResponse> GetAsync(long id);
    }
}