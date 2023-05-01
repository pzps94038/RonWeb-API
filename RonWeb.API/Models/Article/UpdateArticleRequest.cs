using RonWeb.API.Models.Shared;

namespace RonWeb.API.Models.Article
{
    public class UpdateArticleRequest
    {
        public string ArticleTitle { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string CategoryId { get; set; } = string.Empty;
        public List<Label> Labels { get; set; } = new List<Label>();
    }
}
