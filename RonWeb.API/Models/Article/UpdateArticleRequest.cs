﻿using RonWeb.API.Models.Shared;

namespace RonWeb.API.Models.Article
{
    public class UpdateArticleRequest
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
        /// 文章內容
        /// </summary>
        public string Content { get; set; } = string.Empty;
        /// <summary>
        /// 文章預覽內容
        /// </summary>
        public string PreviewContent { get; set; } = string.Empty;
        /// <summary>
        /// 分類ID
        /// </summary>
        public string CategoryId { get; set; } = string.Empty;
    }
}
