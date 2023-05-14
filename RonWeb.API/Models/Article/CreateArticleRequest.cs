using RonWeb.API.Models.Shared;

namespace RonWeb.API.Models.Article
{
    public class CreateArticleRequest
    {
        /// <summary>
        ///  文章標題
        /// </summary>
        public string ArticleTitle { get; set; } = string.Empty;
        /// <summary>
        ///  預覽內容
        /// </summary>
        public string PreviewContent { get; set; } = string.Empty;
        /// <summary>
        /// 內容
        /// </summary>
        public string Content { get; set; } = string.Empty;
        /// <summary>
        /// 分類ID
        /// </summary>
        public string CategoryId { get; set; } = string.Empty;
        /// <summary>
        /// 創建人
        /// </summary>
        public string UserId { get; set; } = string.Empty;
    }
}
