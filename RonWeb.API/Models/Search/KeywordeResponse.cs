using System;
using RonWeb.API.Models.Shared;

namespace RonWeb.API.Models.Search
{
	public class KeywordeResponse
	{
		/// <summary>
		/// 總數
		/// </summary>
		public int Total { get; set; } = 0;

		/// <summary>
		/// 文章列表
		/// </summary>
		public List<ArticleItem> Articles { get; set; } = new List<ArticleItem>();

		/// <summary>
		/// 關鍵字
		/// </summary>
		public string Keyword { get; set; } = string.Empty;
    }
}

