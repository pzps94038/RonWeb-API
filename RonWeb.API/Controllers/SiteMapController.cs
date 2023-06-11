using Microsoft.AspNetCore.Mvc;
using RonWeb.API.Interface.Shared;
using RonWeb.API.Interface.SiteMap;
using RonWeb.API.Models.Shared;
using RonWeb.API.Models.SiteMap;
using RonWeb.Core;

namespace RonWeb.API.Controllers
{
    [Route("api/[controller]/[action]")]
    public class SiteMapController : Controller
    {
        private readonly ISiteMapHelper _helper;
        private readonly ILogHelper _logger;
        public SiteMapController(ISiteMapHelper helper, ILogHelper logger)
        {
            this._helper = helper;
            this._logger = logger;
        }

        /// <summary>
        /// SiteMap 文章ID列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<BaseResponse<List<SiteMapResponse<long>>>> Article()
        {
            var result = new BaseResponse<List<SiteMapResponse<long>>>();
            try
            {
                var data = await this._helper.Article();
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

        /// <summary>
        /// SiteMap 分類ID列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<BaseResponse<List<SiteMapResponse<long>>>> Category()
        {
            var result = new BaseResponse<List<SiteMapResponse<long>>>();
            try
            {
                var data = await this._helper.Category();
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


        /// <summary>
        /// SiteMap 標籤ID列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<BaseResponse<List<SiteMapResponse<long>>>> Label()
        {
            var result = new BaseResponse<List<SiteMapResponse<long>>>();
            try
            {
                var data = await this._helper.Label();
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

