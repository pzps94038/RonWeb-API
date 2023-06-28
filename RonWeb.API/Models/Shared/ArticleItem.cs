using System;
using RonWeb.API.Models.Shared;

namespace RonWeb.API.Models.Shared
{
	public class ArticleItem
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
        /// 內容
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
        /// 是否啟用
        /// </summary>
        public string Flag { get; set; } = "Y";

        /// <summary>
        /// 創建時間
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 標籤列表
        /// </summary>
        public List<Label> Labels { get; set; } = new List<Label>();
    }
}

