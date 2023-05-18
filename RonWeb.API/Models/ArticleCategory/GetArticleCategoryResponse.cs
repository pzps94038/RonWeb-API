using RonWeb.API.Models.Shared;

namespace RonWeb.API.Models.ArticleCategory
{
    public class GetArticleCategoryResponse
    {
        /// <summary>
        /// 分類總數
        /// </summary>
        public int Total { get; set; } = 0;
        /// <summary>
        /// 文章列表
        /// </summary>
        public List<Category> Categorys { get; set; } = new List<Category>();
    }
}
