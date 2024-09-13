using Microsoft.AspNetCore.Mvc;
using RonWeb.API.Interface.ArticleCategory;
using RonWeb.API.Interface.Shared;
using RonWeb.API.Models.ArticleCategory;
using RonWeb.API.Models.Shared;
using RonWeb.Core;

namespace RonWeb.API.Controllers
{
    /// <summary>
    /// 文章分類
    /// </summary>
    [Route("api/[controller]")]
    public class ArticleCategoryController : Controller
    {
        private readonly IArticleCategoryHelper _helper;
        private readonly ILogHelper _logger;
        public ArticleCategoryController(IArticleCategoryHelper helper, ILogHelper logger)
        {
            _helper = helper;
            _logger = logger;
        }

        /// <summary>
        /// 取得分類
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<BaseResponse<GetArticleCategoryResponse>> GetArticleCategory(int? page)
        {
            var result = new BaseResponse<GetArticleCategoryResponse>();
            var data = await _helper.GetListAsync(page);
            result.ReturnCode = ReturnCode.Success.Description();
            result.ReturnMessage = ReturnMessage.Success.Description();
            result.Data = data;
            return result;
        }
    }
}

