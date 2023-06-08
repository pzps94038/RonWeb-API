using Microsoft.AspNetCore.Mvc;
using RonWeb.API.Interface.Search;
using RonWeb.API.Models.Search;
using RonWeb.API.Models.CustomizeException;
using RonWeb.API.Models.Shared;
using RonWeb.Core;
using RonWeb.API.Interface.Shared;

namespace RonWeb.API.Controllers
{
    /// <summary>
    /// 搜尋相關
    /// </summary>
    [Route("api/[controller]")]
    public class SearchController : Controller
    {
        private readonly ISearchHelper _helper;
        private readonly ILogHelper _logger;
        public SearchController(ISearchHelper helper, ILogHelper logger)
        {
            this._helper = helper;
            this._logger = logger;
        }

        /// <summary>
        /// 分類查詢
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<BaseResponse<KeywordeResponse>> Category(long id, int? page)
        {
            var result = new BaseResponse<KeywordeResponse>();
            try
            {
                var data = await this._helper.Category(id, page);
                result.ReturnCode = ReturnCode.Success.Description();
                result.ReturnMessage = ReturnMessage.Success.Description();
                result.Data = data;
            }
            catch(NotFoundException ex)
            {
                result.ReturnCode = ReturnCode.NotFound.Description();
                result.ReturnMessage = ReturnMessage.NotFound.Description();
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
        /// 分類查詢
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<BaseResponse<KeywordeResponse>> Label(long id, int? page)
        {
            var result = new BaseResponse<KeywordeResponse>();
            try
            {
                var data = await this._helper.Label(id, page);
                result.ReturnCode = ReturnCode.Success.Description();
                result.ReturnMessage = ReturnMessage.Success.Description();
                result.Data = data;
            }
            catch (NotFoundException ex)
            {
                result.ReturnCode = ReturnCode.NotFound.Description();
                result.ReturnMessage = ReturnMessage.NotFound.Description();
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

