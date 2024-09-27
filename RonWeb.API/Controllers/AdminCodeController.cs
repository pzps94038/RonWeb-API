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
    public class AdminCodeController : Controller
    {
        private readonly IAdminCodeHelper _helper;
        public AdminCodeController(IAdminCodeHelper helper)
        {
            _helper = helper;
        }

        /// <summary>
        /// 取得代碼類型列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<BaseResponse<GetCodeResponse>> GetCodeType(string codeTypeId, int? page)
        {
            var result = new BaseResponse<GetCodeResponse>();
            var data = await _helper.GetListAsync(codeTypeId, page);
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
        public async Task<BaseResponse<Code>> GetCodeTypeById(long id)
        {
            var result = new BaseResponse<Code>();
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
        [HttpPost]
        public async Task<BaseResponse> CreateArticle([FromBody] CreateCodeRequest data)
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
        public async Task<BaseResponse> UpdateArticle(long id, [FromBody] UpdateCodeRequest data)
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
        public async Task<BaseResponse> DeleteArticle(long id)
        {
            var result = new BaseResponse();
            await _helper.DeleteAsync(id);
            result.ReturnCode = ReturnCode.Success.Description();
            result.ReturnMessage = ReturnMessage.DeleteSuccess.Description();
            return result;
        }
    }
}

