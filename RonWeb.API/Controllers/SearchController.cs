using Microsoft.AspNetCore.Mvc;
using RonWeb.API.Interface.Search;
using RonWeb.API.Models.Search;
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
        public SearchController(ISearchHelper helper)
        {
            _helper = helper;
        }

        /// <summary>
        /// 分類查詢
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<BaseResponse<KeywordeResponse>> Article(long id, int? page)
        {
            var result = new BaseResponse<KeywordeResponse>();
            var data = await _helper.Category(id, page);
            result.ReturnCode = ReturnCode.Success.Description();
            result.ReturnMessage = ReturnMessage.Success.Description();
            result.Data = data;
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
        public async Task<BaseResponse<KeywordeResponse>> Category(long id, int? page)
        {
            var result = new BaseResponse<KeywordeResponse>();
            var data = await _helper.Category(id, page);
            result.ReturnCode = ReturnCode.Success.Description();
            result.ReturnMessage = ReturnMessage.Success.Description();
            result.Data = data;
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
            var data = await _helper.Label(id, page);
            result.ReturnCode = ReturnCode.Success.Description();
            result.ReturnMessage = ReturnMessage.Success.Description();
            result.Data = data;
            return result;
        }
    }
}

