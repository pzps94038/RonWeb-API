using System;
using RonWeb.API.Models.Article;

namespace RonWeb.API.Interface.AdminArticleHelper
{
    public interface IAdminArticleHelper
    {
        public Task<GetArticleResponse> GetListAsync(int? page, string? keyword);
        public Task<GetByIdArticleResponse> GetAsync(long id);
        public Task CreateAsync(CreateArticleRequest data);
        public Task UpdateAsync(long id, UpdateArticleRequest data);
        public Task DeleteAsync(long data);
    }
}

