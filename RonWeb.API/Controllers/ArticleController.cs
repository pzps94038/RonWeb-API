using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RonWeb.API.Enum;
using RonWeb.API.Helper.Shared;
using RonWeb.API.Interface.Article;
using RonWeb.API.Models.Article;
using RonWeb.API.Models.CustomizeException;
using RonWeb.API.Models.Shared;
using RonWeb.Core;

namespace RonWeb.API.Controllers
{
    [Route("api/[controller]")]
    public class ArticleController : Controller
    {
        private readonly IArticleHelper _helper;
        public ArticleController(IArticleHelper helper)
        {
            this._helper = helper;
        }

        [HttpGet]
        public async Task<BaseResponse<List<ArticleItem>>> Get(int limit, int offset, OrderEnum order, string? keyword)
        {
            var result = new BaseResponse<List<ArticleItem>>();
            try
            {
                var data = await this._helper.GetListAsync(limit, offset, order, keyword);
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

        [HttpGet("{id}")]
        public async Task<BaseResponse<GetByIdArticle>> Get(string id)
        {
            var result = new BaseResponse<GetByIdArticle>();
            try
            {
                var data = await this._helper.GetAsync(id);
                result.ReturnCode = ReturnCode.Success.Description();
                result.ReturnMessage = ReturnMessage.Success.Description();
                result.Data = data;
            }
            catch (NotFoundException ex)
            {
                result.ReturnCode = ReturnCode.NotFound.Description();
                result.ReturnMessage = ReturnMessage.NotFound.Description();
                MongoLogHelper.Warn(ex);
            }
            catch (Exception ex)
            {
                result.ReturnCode = ReturnCode.Fail.Description();
                result.ReturnMessage = ReturnMessage.Fail.Description();
                MongoLogHelper.Error(ex);
            }
            return result;
        }
    }
}

