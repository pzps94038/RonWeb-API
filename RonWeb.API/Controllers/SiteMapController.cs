using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RonWeb.API.Helper.Shared;
using RonWeb.API.Interface.SiteMap;
using RonWeb.API.Interface.Upload;
using RonWeb.API.Models.Search;
using RonWeb.API.Models.Shared;
using RonWeb.API.Models.SiteMap;
using RonWeb.Core;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RonWeb.API.Controllers
{
    [Route("api/[controller]/[action]")]
    public class SiteMapController : Controller
    {
        private readonly ISiteMapHelper _helper;
        public SiteMapController(ISiteMapHelper helper)
        {
            this._helper = helper;
        }

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
                LogHelper.Error(ex);
            }

            return result;
        }

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
                LogHelper.Error(ex);
            }

            return result;
        }

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
                LogHelper.Error(ex);
            }

            return result;
        }
    }
}

