using System;
using RonWeb.API.Models.ArticleLabel;

namespace RonWeb.API.Interface.AdminArticleLabel
{
    public interface IAdminArticleLabelUpdate
    {
        public Task UpdateAsync(long id, UpdateArticleLabelRequest data);
    }
}