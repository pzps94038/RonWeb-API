using RonWeb.API.Models.Shared;

namespace RonWeb.API.Models.Article
{
    public class CreateArticleRequest
    {
        public string ArticleTitle { get; set; } = string.Empty;
        public string PreviewContent { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string CategoryId { get; set; } = string.Empty;
        public List<Label> Labels { get; set; } = new List<Label>();
        public string CreateBy { get; set; } = string.Empty;
    }
}
