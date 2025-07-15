using System;
using RonWeb.API.Models.Article;

namespace RonWeb.API.Interface.Article
{
    public interface IArticleGet
    {
        public Task<GetByIdArticleResponse> GetAsync(long id);
    }
}