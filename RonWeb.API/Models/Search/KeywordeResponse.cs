using System;
using RonWeb.API.Models.Shared;

namespace RonWeb.API.Models.Search
{
	public class KeywordeResponse
	{
		public int Total { get; set; } = 0;
		public List<ArticleItem> Articles = new List<ArticleItem>();
		public string Keyword { get; set; } = string.Empty;
    }
}

