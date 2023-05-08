using System;
using RonWeb.API.Models.Shared;

namespace RonWeb.API.Models.Article
{
	public class GetByIdArticleResponse
    {
		public string ArticleId { get; set; } = string.Empty;
		public string ArticleTitle { get; set; } = string.Empty;
		public string PreviewContent { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
		public string CategoryId { get; set; } = string.Empty;
		public string CategoryName { get; set; } = string.Empty;
        public int ViewCount { get; set; } = 0;
		public DateTime CreateDate { get; set; }
    }
}

