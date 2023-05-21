using System;
using RonWeb.API.Models.Shared;

namespace RonWeb.API.Models.Article
{
	public class GetByIdArticleResponse
    {
		/// <summary>
		/// 文章ID
		/// </summary>
		public long ArticleId { get; set; }

		/// <summary>
		/// 文章標題
		/// </summary>
		public string ArticleTitle { get; set; } = string.Empty;

		/// <summary>
		/// 預覽內容
		/// </summary>
		public string PreviewContent { get; set; } = string.Empty;

		/// <summary>
		/// 文章內容
		/// </summary>
        public string Content { get; set; } = string.Empty;

		/// <summary>
		/// 分類ID
		/// </summary>
		public long CategoryId { get; set; }

		/// <summary>
		/// 分類名稱
		/// </summary>
		public string CategoryName { get; set; } = string.Empty;

		/// <summary>
		/// 瀏覽次數
		/// </summary>
        public int ViewCount { get; set; } = 0;

        /// <summary>
        /// 標籤列表
        /// </summary>
        public List<Label> Labels { get; set; } = new List<Label>();

        /// <summary>
        /// 創建日期
        /// </summary>
        public DateTime CreateDate { get; set; }

		/// <summary>
		/// 上一個文章資訊
		/// </summary>
		public BlogPagination? PrevArticle { get; set; }

		/// <summary>
		/// 下一個文章資訊
		/// </summary>
		public BlogPagination? NextArticle { get; set; }
    }
}

