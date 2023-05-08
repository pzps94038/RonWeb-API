using System;
using RonWeb.API.Models.Shared;

namespace RonWeb.API.Models.Article
{
	public class GetArticleResponse
	{
		public int Total { get; set; } = 0;
		public List<ArticleItem> Articles { get; set; } = new List<ArticleItem>();
    }
}

