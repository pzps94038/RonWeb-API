using System;
using RonWeb.API.Interface.Shared;
using RonWeb.API.Models.ArticleLabel;
using RonWeb.API.Models.Shared;

namespace RonWeb.API.Interface.AdminArticleLabel
{
	public interface IAdminArticleLabelHelper :
        IGetAsync<long, Label>,
        ICreateAsync<CreateArticleLabelRequest>,
        IUpdateAsync<long, UpdateArticleLabelRequest>,
        IDeleteAsync<long>
    {
        public Task<GetArticleLabelResponse> GetListAsync(int? page);
    }
}

