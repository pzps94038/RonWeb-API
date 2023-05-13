using System;
using RonWeb.API.Models.Shared;

namespace RonWeb.API.Models.Shared
{
	public class ArticleItem
	{
        /// <summary>
        /// 文章ID
        /// </summary>
        public string ArticleId { get; set; } = string.Empty;
        /// <summary>
        /// 文章標題
        /// </summary>
        public string ArticleTitle { get; set; } = string.Empty;
        /// <summary>
        /// 預覽內容
        /// </summary>
        public string PreviewContent { get; set; } = string.Empty;
        /// <summary>
        /// 分類ID
        /// </summary>
        public string CategoryId { get; set; } = string.Empty;
        /// <summary>
        /// 分類名稱
        /// </summary>
        public string CategoryName { get; set; } = string.Empty;
        /// <summary>
        /// 瀏覽次數
        /// </summary>
        public int ViewCount { get; set; } = 0;
        /// <summary>
        /// 創建時間
        /// </summary>
        public DateTime CreateDate { get; set; }
    }
}

