using RonWeb.API.Models.Shared;

namespace RonWeb.API.Models.ArticleCategory
{
    public class UpdateArticleCategoryRequest: Category
    {
        /// <summary>
        /// 更新人
        /// </summary>
        public string UserId { get; set; } = string.Empty;
    }
}
