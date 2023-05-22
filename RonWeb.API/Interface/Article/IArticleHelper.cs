using System;
using RonWeb.API.Enum;
using RonWeb.API.Interface.Shared;
using RonWeb.API.Models.Article;

namespace RonWeb.API.Interface.Article
{
	public interface IArticleHelper :
		IGetAsync<long, GetByIdArticleResponse>,
		IDeleteAsync<long>,
		IUpdateAsync<long, UpdateArticleRequest>,
		ICreateAsync<CreateArticleRequest>
	{
		public Task<GetArticleResponse> GetListAsync(int? page, string? keyword);
		public Task UpdateArticleViews(long id);

    }
}

