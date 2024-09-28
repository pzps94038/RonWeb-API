using RonWeb.API.Interface.Shared;
using RonWeb.API.Models.Article;
using RonWeb.API.Models.ArticleCategory;
using RonWeb.API.Models.Shared;

namespace RonWeb.API.Interface.ArticleCategory
{
    public interface IArticleCategoryHelper
    {
        public Task<GetArticleCategoryResponse> GetListAsync();
    }
}
