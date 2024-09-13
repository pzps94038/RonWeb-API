using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RonWeb.API.Interface.AdminArticleLabel;
using RonWeb.API.Interface.Shared;
using RonWeb.API.Models.ArticleLabel;
using RonWeb.API.Models.CustomizeException;
using RonWeb.API.Models.Shared;
using RonWeb.Core;

namespace RonWeb.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class AdminArticleLabelController : Controller
    {
        private readonly IAdminArticleLabelHelper _helper;
        private readonly ILogHelper _logger;
        public AdminArticleLabelController(IAdminArticleLabelHelper helper, ILogHelper logger)
        {
            _helper = helper;
            _logger = logger;
        }

        /// <summary>
        /// 取得標籤
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<BaseResponse<GetArticleLabelResponse>> GetArticleLabel(int? page)
        {
            var result = new BaseResponse<GetArticleLabelResponse>();
            var data = await _helper.GetListAsync(page);
            result.ReturnCode = ReturnCode.Success.Description();
            result.ReturnMessage = ReturnMessage.Success.Description();
            result.Data = data;
            return result;
        }


        /// <summary>
        /// 取得指定標籤
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<BaseResponse<Label>> GetArticleLabelById(long id)
        {
            var result = new BaseResponse<Label>();
            var data = await _helper.GetAsync(id);
            result.ReturnCode = ReturnCode.Success.Description();
            result.ReturnMessage = ReturnMessage.Success.Description();
            result.Data = data;
            return result;
        }

        /// <summary>
        /// 建立標籤
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<BaseResponse> CreateArticleLabel([FromBody] CreateArticleLabelRequest data)
        {
            var result = new BaseResponse();
            await _helper.CreateAsync(data);
            result.ReturnCode = ReturnCode.Success.Description();
            result.ReturnMessage = ReturnMessage.CreateSuccess.Description();
            return result;
        }

        /// <summary>
        /// 修改標籤
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        public async Task<BaseResponse> UpdateArticleLabel(long id, [FromBody] UpdateArticleLabelRequest data)
        {
            var result = new BaseResponse();
            await _helper.UpdateAsync(id, data);
            result.ReturnCode = ReturnCode.Success.Description();
            result.ReturnMessage = ReturnMessage.ModifySuccess.Description();
            return result;
        }

        /// <summary>
        /// 刪除標籤
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<BaseResponse> DeleteArticleLabel(long id)
        {
            var result = new BaseResponse();
            await _helper.DeleteAsync(id);
            result.ReturnCode = ReturnCode.Success.Description();
            result.ReturnMessage = ReturnMessage.DeleteSuccess.Description();
            return result;
        }
    }
}

