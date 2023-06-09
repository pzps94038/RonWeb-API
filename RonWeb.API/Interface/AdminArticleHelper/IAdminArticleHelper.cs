using System;
using RonWeb.API.Interface.Shared;
using RonWeb.API.Models.Article;

namespace RonWeb.API.Interface.AdminArticleHelper
{
	public interface IAdminArticleHelper : IGetAsync<long, GetByIdArticleResponse>,

        IDeleteAsync<long>,
        IUpdateAsync<long, UpdateArticleRequest>,
        ICreateAsync<CreateArticleRequest>
    {
        public Task<GetArticleResponse> GetListAsync(int? page, string? keyword);
    }
}

