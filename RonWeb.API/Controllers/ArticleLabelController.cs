using Microsoft.AspNetCore.Mvc;
using RonWeb.API.Interface.ArticleLabel;
using RonWeb.API.Interface.Shared;
using RonWeb.API.Models.ArticleLabel;
using RonWeb.API.Models.Shared;
using RonWeb.Core;

namespace RonWeb.API.Controllers
{
    [Route("api/[controller]")]
    public class ArticleLabelController : Controller
    {
        private readonly IArticleLabelHelper _helper;
        private readonly ILogHelper _logger;
        public ArticleLabelController(IArticleLabelHelper helper, ILogHelper logger)
        {
            _helper = helper;
            _logger = logger;
        }

        /// <summary>
        /// 取得標籤
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<BaseResponse<GetArticleLabelResponse>> GetArticleLabel(int? page)
        {
            var result = new BaseResponse<GetArticleLabelResponse>();
            var data = await _helper.GetListAsync(page);
            result.ReturnCode = ReturnCode.Success.Description();
            result.ReturnMessage = ReturnMessage.Success.Description();
            result.Data = data;
            return result;
        }
    }
}

