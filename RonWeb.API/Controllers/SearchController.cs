using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RonWeb.API.Helper.Shared;
using RonWeb.API.Interface.Article;
using RonWeb.API.Interface.Search;
using RonWeb.API.Models.Search;
using RonWeb.API.Models.CustomizeException;
using RonWeb.API.Models.Shared;
using RonWeb.Core;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
            this._helper = helper;
        }

        /// <summary>
        /// 關鍵字查詢
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]/{keyword}")]
        public async Task<BaseResponse<KeywordeResponse>> Keyword(string keyword, int? page)
        {
            var result = new BaseResponse<KeywordeResponse>();
            try
            {
                var data = await this._helper.Keyword(keyword, page);
                result.ReturnCode = ReturnCode.Success.Description();
                result.ReturnMessage = ReturnMessage.Success.Description();
                result.Data = data;
            }
            catch (Exception ex)
            {
                result.ReturnCode = ReturnCode.Fail.Description();
                result.ReturnMessage = ReturnMessage.Fail.Description();
                LogHelper.Error(ex);
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
                LogHelper.Error(ex);
            }

            return result;
        }
    }
}

