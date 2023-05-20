using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RonWeb.API.Models.ArticleLabel;
using RonWeb.API.Models.Shared;
using RonWeb.Core;

namespace RonWeb.API.Controllers
{
    [Route("api/[controller]")]
    public class ArticleLabelController : Controller
    {
        /// <summary>
        /// 取得分類
        /// </summary>
        /// <returns></returns>
        //[HttpGet]
        //public async Task<BaseResponse<GetArticleLabelResponse>> Get(int? page)
        //{
        //    var result = new BaseResponse<GetArticleLabelResponse>();
        //    try
        //    {
        //        var data = await this._helper.GetListAsync(page);
        //        result.ReturnCode = ReturnCode.Success.Description();
        //        result.ReturnMessage = ReturnMessage.Success.Description();
        //        result.Data = data;
        //    }
        //    catch (Exception ex)
        //    {
        //        result.ReturnCode = ReturnCode.Fail.Description();
        //        result.ReturnMessage = ReturnMessage.Fail.Description();
        //        MongoLogHelper.Error(ex);
        //    }
        //    return result;
        //}

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPatch("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
    }
}

