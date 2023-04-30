using System;
namespace RonWeb.API.Models.Article
{
	public class ArticleModel
	{
		public string ArticleId { get; set; } = string.Empty;
		public string ArticleTitle { get; set; } = string.Empty;
		public string Content { get; set; } = string.Empty;
		public string CategoryId { get; set; } = string.Empty;
		public string CategoryName { get; set; } = string.Empty;
        public int ViewCount { get; set; } = 0;
		public List<LabelModel> Labels { get; set; } = new List<LabelModel>();
		public DateTime CreateDate { get; set; }
		public string CreateBy { get; set; } = string.Empty;
		public DateTime? UpdateDate { get; set; }
		public string? UpdateBy { get; set; }
    }
}

