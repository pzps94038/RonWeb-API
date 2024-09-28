using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RonWeb.API.Interface.AdminArticleCategory;
using RonWeb.API.Models.ArticleCategory;
using RonWeb.API.Models.CustomizeException;
using RonWeb.API.Models.Shared;
using RonWeb.Core;

namespace RonWeb.API.Controllers
{
    /// <summary>
    /// 管理員文章分類
    /// </summary>
    [Route("api/[controller]")]
    [Authorize]
    public class AdminArticleCategoryController : Controller
    {
        private readonly IAdminArticleCategoryHelper _helper;
        public AdminArticleCategoryController(IAdminArticleCategoryHelper helper)
        {
            _helper = helper;
        }

        /// <summary>
        /// 取得分類
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<BaseResponse<GetArticleCategoryResponse>> GetArticleCategory(int? page)
        {
            var result = new BaseResponse<GetArticleCategoryResponse>();
            var data = await _helper.GetListAsync(page);
            result.ReturnCode = ReturnCode.Success.Description();
            result.ReturnMessage = ReturnMessage.Success.Description();
            result.Data = data;
            return result;
        }


        /// <summary>
        /// 取得指定分類
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<BaseResponse<Category>> GetArticleCategoryById(long id)
        {
            var result = new BaseResponse<Category>();
            try
            {
                var data = await _helper.GetAsync(id);
                result.ReturnCode = ReturnCode.Success.Description();
                result.ReturnMessage = ReturnMessage.Success.Description();
                result.Data = data;
            }
            catch (NotFoundException)
            {
                result.ReturnCode = ReturnCode.NotFound.Description();
                result.ReturnMessage = ReturnMessage.NotFound.Description();
            }
            return result;
        }

        /// <summary>
        /// 建立分類
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<BaseResponse> CreateArticleCategory([FromBody] CreateArticleCategoryRequest data)
        {
            var result = new BaseResponse();
            await _helper.CreateAsync(data);
            result.ReturnCode = ReturnCode.Success.Description();
            result.ReturnMessage = ReturnMessage.CreateSuccess.Description();
            return result;
        }

        /// <summary>
        /// 修改分類
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        public async Task<BaseResponse> UpdateArticleCategory(long id, [FromBody] UpdateArticleCategoryRequest data)
        {
            var result = new BaseResponse();
            await _helper.UpdateAsync(id, data);
            result.ReturnCode = ReturnCode.Success.Description();
            result.ReturnMessage = ReturnMessage.ModifySuccess.Description();
            return result;
        }

        /// <summary>
        /// 刪除分類
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<BaseResponse> DeleteArticleCategory(long id)
        {
            var result = new BaseResponse();
            await _helper.DeleteAsync(id);
            result.ReturnCode = ReturnCode.Success.Description();
            result.ReturnMessage = ReturnMessage.DeleteSuccess.Description();
            return result;
        }
    }
}

