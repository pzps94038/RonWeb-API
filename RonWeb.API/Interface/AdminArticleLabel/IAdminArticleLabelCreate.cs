using System;
using RonWeb.API.Models.ArticleLabel;

namespace RonWeb.API.Interface.AdminArticleLabel
{
    public interface IAdminArticleLabelCreate
    {
        public Task CreateAsync(CreateArticleLabelRequest data);
    }
}