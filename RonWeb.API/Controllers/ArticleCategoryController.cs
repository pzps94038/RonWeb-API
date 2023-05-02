using Microsoft.AspNetCore.Mvc;
using RonWeb.API.Helper.Shared;
using RonWeb.API.Interface.ArticleCategory;
using RonWeb.API.Models.ArticleCategory;
using RonWeb.API.Models.CustomizeException;
using RonWeb.API.Models.Shared;
using RonWeb.Core;

namespace RonWeb.API.Controllers
{
    [Route("api/[controller]")]
    public class ArticleCategoryController : Controller
    {
        private readonly IArticleCategoryHelper _helper;
        public ArticleCategoryController(IArticleCategoryHelper helper) 
        {
            this._helper = helper;
        }

        /// <summary>
        /// 取得所有分類
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<BaseResponse<List<Category>>> Get()
        {
            var result = new BaseResponse<List<Category>>();
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

        /// <summary>
        /// 建立分類
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<BaseResponse> Post([FromBody]CreateArticleCategoryRequest data)
        {
            var result = new BaseResponse();
            try
            {
                await this._helper.CreateAsync(data);
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

        /// <summary>
        /// 修改分類
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        public async Task<BaseResponse> Patch(string id, [FromBody]Category data)
        {
            var result = new BaseResponse();
            try
            {
                await this._helper.UpdateAsync(id, data);
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

        /// <summary>
        /// 刪除分類
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

