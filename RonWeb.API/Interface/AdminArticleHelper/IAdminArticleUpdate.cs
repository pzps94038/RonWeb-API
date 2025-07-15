using System;
using RonWeb.API.Models.Article;

namespace RonWeb.API.Interface.AdminArticleHelper
{
    public interface IAdminArticleUpdate
    {
        public Task UpdateAsync(long id, UpdateArticleRequest data);
    }
}