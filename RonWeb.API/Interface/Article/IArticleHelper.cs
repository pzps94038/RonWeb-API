using System;
using RonWeb.API.Interface.Shared;
using RonWeb.API.Models.Article;

namespace RonWeb.API.Interface.Article
{
	public interface IArticleHelper: IGetByIdAsync<ArticleModel>
	{
	}
}

