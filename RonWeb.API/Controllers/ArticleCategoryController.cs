using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RonWeb.API.Filter;
using RonWeb.API.Interface.ArticleCategory;
using RonWeb.API.Interface.Shared;
using RonWeb.API.Models.ArticleCategory;
using RonWeb.API.Models.CustomizeException;
using RonWeb.API.Models.Shared;
using RonWeb.Core;

namespace RonWeb.API.Controllers
{
    /// <summary>
    /// 文章分類
    /// </summary>
    [Route("api/[controller]")]
    [ServiceFilter(typeof(HostFilter))]
    public class ArticleCategoryController : Controller
    {
        private readonly IArticleCategoryHelper _helper;
        private readonly ILogHelper _logger;
        public ArticleCategoryController(IArticleCategoryHelper helper, ILogHelper logger)
        {
            this._helper = helper;
            this._logger = logger;
        }

        /// <summary>
        /// 取得分類
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<BaseResponse<GetArticleCategoryResponse>> GetArticleCategory(int? page)
        {
            var result = new BaseResponse<GetArticleCategoryResponse>();
            try
            {
                var data = await this._helper.GetListAsync(page);
                result.ReturnCode = ReturnCode.Success.Description();
                result.ReturnMessage = ReturnMessage.Success.Description();
                result.Data = data;
            }
            catch (Exception ex)
            {
                result.ReturnCode = ReturnCode.Fail.Description();
                result.ReturnMessage = ReturnMessage.Fail.Description();
                _logger.Error(ex);
            }
            return result;
        }
    }
}

