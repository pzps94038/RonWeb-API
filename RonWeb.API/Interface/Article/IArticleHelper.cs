using System;
using RonWeb.API.Enum;
using RonWeb.API.Models.Article;

namespace RonWeb.API.Interface.Article
{
	public interface IArticleHelper
	{
		public Task<GetArticleResponse> GetListAsync(int? page, string? keyword);
		public Task UpdateArticleViews(long id);
		public Task<GetByIdArticleResponse> GetAsync(long id);
    }
}

