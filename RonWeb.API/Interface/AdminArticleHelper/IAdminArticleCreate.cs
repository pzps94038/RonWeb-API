using System;
using RonWeb.API.Models.Article;

namespace RonWeb.API.Interface.AdminArticleHelper
{
    public interface IAdminArticleCreate
    {
        public Task CreateAsync(CreateArticleRequest data);
    }
}