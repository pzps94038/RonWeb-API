using RonWeb.API.Interface.Shared;
using RonWeb.API.Models.ArticleLabel;
using RonWeb.API.Models.Shared;

namespace RonWeb.API.Interface.ArticleLabel
{
	public interface IArticleLabelHelper
    {
        public Task<GetArticleLabelResponse> GetListAsync(int? page);
    }
}

