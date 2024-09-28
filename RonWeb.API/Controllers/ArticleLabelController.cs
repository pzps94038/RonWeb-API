using Microsoft.AspNetCore.Mvc;
using RonWeb.API.Interface.ArticleLabel;
using RonWeb.API.Models.ArticleLabel;
using RonWeb.API.Models.Shared;
using RonWeb.Core;

namespace RonWeb.API.Controllers
{
    /// <summary>
    /// 文章標籤
    /// </summary>
    [Route("api/[controller]")]
    public class ArticleLabelController : Controller
    {
        private readonly IArticleLabelHelper _helper;
        public ArticleLabelController(IArticleLabelHelper helper)
        {
            _helper = helper;
        }

        /// <summary>
        /// 取得標籤
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<BaseResponse<GetArticleLabelResponse>> GetArticleLabel()
        {
            var result = new BaseResponse<GetArticleLabelResponse>();
            var data = await _helper.GetListAsync();
            result.ReturnCode = ReturnCode.Success.Description();
            result.ReturnMessage = ReturnMessage.Success.Description();
            result.Data = data;
            return result;
        }
    }
}

