using System;
namespace RonWeb.API.Models.Article
{
	public class ArticleItem
	{
        public string ArticleId { get; set; } = string.Empty;
        public string ArticleTitle { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string CategoryId { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public int ViewCount { get; set; } = 0;
        public List<Label> Labels { get; set; } = new List<Label>();
        public DateTime CreateDate { get; set; }
    }
}

