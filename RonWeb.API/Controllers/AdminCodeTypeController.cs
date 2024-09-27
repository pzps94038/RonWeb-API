using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RonWeb.API.Models.Article;
using RonWeb.API.Models.Shared;
using RonWeb.Core;
using RonWeb.API.Interface.AdminArticleHelper;
using RonWeb.API.Models.CodeType;
using RonWeb.API.Models.ArticleLabel;
using RonWeb.Database.Entities;
namespace RonWeb.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class AdminCodeTypeController : Controller
    {
        private readonly IAdminCodeTypeHelper _helper;
        public AdminCodeTypeController(IAdminCodeTypeHelper helper)
        {
            _helper = helper;
        }

        /// <summary>
        /// 取得代碼類型列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<BaseResponse<GetCodeTypeResponse>> GetCodeType(int? page)
        {
            var result = new BaseResponse<GetCodeTypeResponse>();
            var data = await _helper.GetListAsync(page);
            result.ReturnCode = ReturnCode.Success.Description();
            result.ReturnMessage = ReturnMessage.Success.Description();
            result.Data = data;
            return result;
        }

        /// <summary>
        /// 取得代碼類型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<BaseResponse<CodeType>> GetCodeTypeById(string id)
        {
            var result = new BaseResponse<CodeType>();
            var data = await _helper.GetAsync(id);
            result.ReturnCode = ReturnCode.Success.Description();
            result.ReturnMessage = ReturnMessage.Success.Description();
            result.Data = data;
            return result;
        }

        /// <summary>
        /// 新增代碼類型
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<BaseResponse> CreateArticle([FromBody] CreateCodeTypeRequest data)
        {
            var result = new BaseResponse();
            await _helper.CreateAsync(data);
            result.ReturnCode = ReturnCode.Success.Description();
            result.ReturnMessage = ReturnMessage.CreateSuccess.Description();
            return result;
        }

        /// <summary>
        /// 修改代碼類型
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        public async Task<BaseResponse> UpdateArticle(string id, [FromBody] UpdateCodeTypeRequest data)
        {
            var result = new BaseResponse();
            await _helper.UpdateAsync(id, data);
            result.ReturnCode = ReturnCode.Success.Description();
            result.ReturnMessage = ReturnMessage.ModifySuccess.Description();
            return result;
        }

        /// <summary>
        /// 刪除代碼類型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<BaseResponse> DeleteArticle(string id)
        {
            var result = new BaseResponse();
            await _helper.DeleteAsync(id);
            result.ReturnCode = ReturnCode.Success.Description();
            result.ReturnMessage = ReturnMessage.DeleteSuccess.Description();
            return result;
        }
    }
}

