using System;
using RonWeb.API.Enum;
using RonWeb.API.Interface.Shared;
using RonWeb.API.Models.Article;

namespace RonWeb.API.Interface.Article
{
	public interface IArticleHelper: IGetAsync<GetByIdArticle, string>
	{
		public Task<List<ArticleItem>> GetListAsync(int limit, int offset, OrderEnum order, string? keyword);
	}
}

