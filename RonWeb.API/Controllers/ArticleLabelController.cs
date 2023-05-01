using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using RonWeb.API.Helper.Shared;
using RonWeb.API.Interface.Article;
using RonWeb.API.Interface.ArticleLabel;
using RonWeb.API.Models.Article;
using RonWeb.API.Models.ArticleLabel;
using RonWeb.API.Models.CustomizeException;
using RonWeb.API.Models.Shared;
using RonWeb.Core;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace RonWeb.API.Controllers
{
    [Route("api/[controller]")]
    public class ArticleLabelController : Controller
    {

        private readonly IArticleLabelHelper _helper;
        public ArticleLabelController(IArticleLabelHelper helper)
        {
            this._helper = helper;
        }

        [HttpGet]
        public async Task<BaseResponse<List<Label>>> Get()
        {
            var result = new BaseResponse<List<Label>>();

            try
            {
                var data = await this._helper.GetListAsync();
                result.ReturnCode = ReturnCode.Success.Description();
                result.ReturnMessage = ReturnMessage.Success.Description();
                result.Data = data;
            }
            catch (Exception ex)
            {
                result.ReturnCode = ReturnCode.Fail.Description();
                result.ReturnMessage = ReturnMessage.Fail.Description();
                MongoLogHelper.Error(ex);
            }

            return result;
        }

        [HttpPost]
        public async Task<BaseResponse> Post([FromBody]CreateArticleLabel value)
        {
            var result = new BaseResponse();

            try
            {
                await this._helper.CreateAsync(value);
                result.ReturnCode = ReturnCode.Success.Description();
                result.ReturnMessage = ReturnMessage.CreateSuccess.Description();
            }
            catch (UniqueException ex)
            {
                result.ReturnCode = ReturnCode.Unique.Description();
                result.ReturnMessage = ReturnMessage.Unique.Description();
                MongoLogHelper.Warn(ex);
            }
            catch (Exception ex)
            {
                result.ReturnCode = ReturnCode.Fail.Description();
                result.ReturnMessage = ReturnMessage.CreateFail.Description();
                MongoLogHelper.Error(ex);
            }

            return result;
        }

        [HttpPatch("{id}")]
        public async Task<BaseResponse> Put(string id, [FromBody]Label data)
        {
            var result = new BaseResponse();

            try
            {
                await this._helper.UpdateAsync(data);
                result.ReturnCode = ReturnCode.Success.Description();
                result.ReturnMessage = ReturnMessage.ModifySuccess.Description();
            }
            catch (Exception ex)
            {
                result.ReturnCode = ReturnCode.Fail.Description();
                result.ReturnMessage = ReturnMessage.ModifyFail.Description();
                MongoLogHelper.Error(ex);
            }

            return result;
        }

        [HttpDelete("{id}")]
        public async Task<BaseResponse> Delete(string id)
        {
            var result = new BaseResponse();

            try
            {
                await this._helper.DeleteAsync(id);
                result.ReturnCode = ReturnCode.Success.Description();
                result.ReturnMessage = ReturnMessage.DeleteSuccess.Description();
            }
            catch (Exception ex)
            {
                result.ReturnCode = ReturnCode.Fail.Description();
                result.ReturnMessage = ReturnMessage.DeleteFail.Description();
                MongoLogHelper.Error(ex);
            }

            return result;
        }
    }
}

