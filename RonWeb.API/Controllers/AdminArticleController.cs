using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RonWeb.API.Models.Article;
using RonWeb.API.Models.CustomizeException;
using RonWeb.API.Models.Shared;
using RonWeb.Core;
using RonWeb.API.Interface.Shared;
using RonWeb.API.Interface.AdminArticleHelper;

namespace RonWeb.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class AdminArticleController : Controller
    {
        private readonly IAdminArticleHelper _helper;
        private readonly ILogHelper _logger;
        public AdminArticleController(IAdminArticleHelper helper, ILogHelper logger)
        {
            _helper = helper;
            _logger = logger;
        }

        /// <summary>
        /// 取得文章列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<BaseResponse<GetArticleResponse>> GetArticle(int? page, string? keyword)
        {
            var result = new BaseResponse<GetArticleResponse>();
            var data = await _helper.GetListAsync(page, keyword);
            result.ReturnCode = ReturnCode.Success.Description();
            result.ReturnMessage = ReturnMessage.Success.Description();
            result.Data = data;
            return result;
        }

        /// <summary>
        /// 取得文章
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<BaseResponse<GetByIdArticleResponse>> GetArticleById(long id)
        {
            var result = new BaseResponse<GetByIdArticleResponse>();
            var data = await _helper.GetAsync(id);
            result.ReturnCode = ReturnCode.Success.Description();
            result.ReturnMessage = ReturnMessage.Success.Description();
            result.Data = data;
            return result;
        }

        /// <summary>
        /// 新增文章
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<BaseResponse> CreateArticle([FromBody] CreateArticleRequest data)
        {
            var result = new BaseResponse();
            await _helper.CreateAsync(data);
            result.ReturnCode = ReturnCode.Success.Description();
            result.ReturnMessage = ReturnMessage.CreateSuccess.Description();
            return result;
        }

        /// <summary>
        /// 修改文章
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        public async Task<BaseResponse> UpdateArticle(long id, [FromBody] UpdateArticleRequest data)
        {
            var result = new BaseResponse();
            await _helper.UpdateAsync(id, data);
            result.ReturnCode = ReturnCode.Success.Description();
            result.ReturnMessage = ReturnMessage.ModifySuccess.Description();
            return result;
        }

        /// <summary>
        /// 刪除文章
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

