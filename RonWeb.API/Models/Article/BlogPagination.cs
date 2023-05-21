using System;
namespace RonWeb.API.Models.Article
{
	public class BlogPagination
	{
        /// <summary>
        /// 文章ID
        /// </summary>
        public long ArticleId { get; set; }

        /// <summary>
        /// 文章標題
        /// </summary>
        public string ArticleTitle { get; set; } = string.Empty;
    }
}

