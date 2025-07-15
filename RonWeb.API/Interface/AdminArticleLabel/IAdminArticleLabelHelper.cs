using System;
using RonWeb.API.Models.ArticleLabel;

namespace RonWeb.API.Interface.AdminArticleLabel
{
    public interface IAdminArticleLabelHelper
    {
        public Task<GetArticleLabelResponse> GetListAsync(int? page);
        public Task<Label> GetAsync(long id);
        public Task CreateAsync(CreateArticleLabelRequest data);
        public Task UpdateAsync(long id, UpdateArticleLabelRequest data);
        public Task DeleteAsync(long data);
    }
}

