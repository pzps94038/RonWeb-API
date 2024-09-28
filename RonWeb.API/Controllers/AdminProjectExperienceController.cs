using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RonWeb.API.Models.Shared;
using RonWeb.Core;
using RonWeb.API.Interface.AdminArticleHelper;
using RonWeb.API.Models.ProjectExperience;
namespace RonWeb.API.Controllers
{
    /// <summary>
    /// 管理員專案經歷
    /// </summary>
    [Route("api/[controller]")]
    [Authorize]
    public class AdminProjectExperienceController : Controller
    {
        private readonly IAdminProjectExperienceHelper _helper;
        public AdminProjectExperienceController(IAdminProjectExperienceHelper helper)
        {
            _helper = helper;
        }

        /// <summary>
        /// 取得專案經歷列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<BaseResponse<GetProjectExperienceResponse>> GetProjectExperience(int? page)
        {
            var result = new BaseResponse<GetProjectExperienceResponse>();
            var data = await _helper.GetListAsync(page);
            result.ReturnCode = ReturnCode.Success.Description();
            result.ReturnMessage = ReturnMessage.Success.Description();
            result.Data = data;
            return result;
        }

        /// <summary>
        /// 取得專案經歷
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<BaseResponse<GetByIdProjectExperienceResponse>> GetProjectExperienceById(long id)
        {
            var result = new BaseResponse<GetByIdProjectExperienceResponse>();
            var data = await _helper.GetAsync(id);
            result.ReturnCode = ReturnCode.Success.Description();
            result.ReturnMessage = ReturnMessage.Success.Description();
            result.Data = data;
            return result;
        }

        /// <summary>
        /// 新增專案經歷
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<BaseResponse> CreateProjectExperience([FromBody] CreateProjectExperienceRequest data)
        {
            var result = new BaseResponse();
            await _helper.CreateAsync(data);
            result.ReturnCode = ReturnCode.Success.Description();
            result.ReturnMessage = ReturnMessage.CreateSuccess.Description();
            return result;
        }

        /// <summary>
        /// 修改專案經歷
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        public async Task<BaseResponse> UpdateProjectExperience(long id, [FromBody] UpdateProjectExperienceRequest data)
        {
            var result = new BaseResponse();
            await _helper.UpdateAsync(id, data);
            result.ReturnCode = ReturnCode.Success.Description();
            result.ReturnMessage = ReturnMessage.ModifySuccess.Description();
            return result;
        }

        /// <summary>
        /// 刪除專案經歷
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

