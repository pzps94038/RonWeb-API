using System;
using RonWeb.API.Models.Shared;

namespace RonWeb.API.Models.Article
{
	public class GetArticleResponse
	{
		/// <summary>
		/// 文章總數
		/// </summary>
		public int Total { get; set; } = 0;
		/// <summary>
		/// 文章列表
		/// </summary>
		public List<ArticleItem> Articles { get; set; } = new List<ArticleItem>();
    }
}

