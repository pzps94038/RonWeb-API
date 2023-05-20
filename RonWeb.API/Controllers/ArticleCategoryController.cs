using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RonWeb.API.Helper.Shared;
using RonWeb.API.Interface.ArticleCategory;
using RonWeb.API.Models.Article;
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
    public class ArticleCategoryController : Controller
    {
        private readonly IArticleCategoryHelper _helper;
        public ArticleCategoryController(IArticleCategoryHelper helper) 
        {
            this._helper = helper;
        }

        /// <summary>
        /// 取得分類
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<BaseResponse<GetArticleCategoryResponse>> Get(int? page)
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
                LogHelper.Error(ex);
            }
            return result;
        }


        /// <summary>
        /// 取得指定分類
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<BaseResponse<Category>> Get(long id)
        {
            var result = new BaseResponse<Category>();
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
        /// 建立分類
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
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
            }
            catch (NotFoundException ex)
            {
                result.ReturnCode = ReturnCode.NotFound.Description();
                result.ReturnMessage = ReturnMessage.NotFound.Description();
            }
            catch (Exception ex)
            {
                result.ReturnCode = ReturnCode.Fail.Description();
                result.ReturnMessage = ReturnMessage.CreateFail.Description();
                LogHelper.Error(ex);
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
        [Authorize]
        public async Task<BaseResponse> Patch(long id, [FromBody]UpdateArticleCategoryRequest data)
        {
            var result = new BaseResponse();
            try
            {
                await this._helper.UpdateAsync(id, data);
                result.ReturnCode = ReturnCode.Success.Description();
                result.ReturnMessage = ReturnMessage.ModifySuccess.Description();
            }
            catch (NotFoundException ex)
            {
                result.ReturnCode = ReturnCode.NotFound.Description();
                result.ReturnMessage = ReturnMessage.NotFound.Description();
            }
            catch (Exception ex)
            {
                result.ReturnCode = ReturnCode.Fail.Description();
                result.ReturnMessage = ReturnMessage.ModifyFail.Description();
                LogHelper.Error(ex);
            }
            return result;
        }

        /// <summary>
        /// 刪除分類
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<BaseResponse> Delete(long id)
        {
            var result = new BaseResponse();
            try
            {
                await this._helper.DeleteAsync(id);
                result.ReturnCode = ReturnCode.Success.Description();
                result.ReturnMessage = ReturnMessage.DeleteSuccess.Description();
            }
            catch (NotFoundException ex)
            {
                result.ReturnCode = ReturnCode.NotFound.Description();
                result.ReturnMessage = ReturnMessage.NotFound.Description();
            }
            catch (Exception ex)
            {
                result.ReturnCode = ReturnCode.Fail.Description();
                result.ReturnMessage = ReturnMessage.DeleteFail.Description();
                LogHelper.Error(ex);
            }
            return result;
        }
    }
}

