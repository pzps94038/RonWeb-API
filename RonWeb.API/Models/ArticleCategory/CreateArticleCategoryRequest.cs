namespace RonWeb.API.Models.ArticleCategory
{
    public class CreateArticleCategoryRequest
    {
        /// <summary>
        /// 分類名稱
        /// </summary>
        public string CategoryName { get; set; } = string.Empty;
        /// <summary>
        /// 建立人
        /// </summary>
        public string UserId { get; set; } = string.Empty;
    }
}
