using RonWeb.API.Interface.Shared;
using RonWeb.API.Models.ArticleCategory;
using RonWeb.API.Models.Shared;

namespace RonWeb.API.Interface.ArticleCategory
{
    public interface IArticleCategoryHelper : IGetListAsync<Category>, ICreateAsync<CreateArticleCategoryRequest>, IUpdateAsync<Category>, IDeleteAsync<string>
    {
    }
}
