using RonWeb.API.Models.Shared;

namespace RonWeb.API.Models.ArticleCategory
{
    public class UpdateArticleCategoryRequest
    {
        /// 分類ID
        /// </summary>
        public long CategoryId { get; set; }

        /// <summary>
        /// 分類名稱
        /// </summary>
        public string CategoryName { get; set; } = string.Empty;

        /// <summary>
        /// 更新人
        /// </summary>
        public long UserId { get; set; }
    }
}
