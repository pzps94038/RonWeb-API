using System;
using RonWeb.API.Models.ArticleLabel;

namespace RonWeb.API.Interface.AdminArticleLabel
{
    public interface IAdminArticleLabelHelper :
        IAdminArticleLabelGet,
        IAdminArticleLabelCreate,
        IAdminArticleLabelUpdate,
        IAdminArticleLabelDelete
    {
        public Task<GetArticleLabelResponse> GetListAsync(int? page);
    }
}

