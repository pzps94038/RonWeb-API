using System;
using RonWeb.API.Models.ArticleLabel;

namespace RonWeb.API.Interface.AdminArticleLabel
{
    public interface IAdminArticleLabelGet
    {
        public Task<Label> GetAsync(long id);
    }
}