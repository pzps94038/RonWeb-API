using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RonWeb.API.Interface.Shared;
using RonWeb.API.Interface.Upload;
using RonWeb.API.Models.Shared;
using RonWeb.API.Models.Upload;
using RonWeb.Core;

namespace RonWeb.API.Controllers
{
    /// <summary>
    /// 檔案上傳
    /// </summary>
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class UploadController : Controller
    {
        private readonly IUploadHelper _helper;
        public UploadController(IUploadHelper helper)
        {
            _helper = helper;
        }

        /// <summary>
        /// 檔案上傳到FireBase
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<BaseResponse<UploadFileResponse>> UploadArticleFile(IFormFile file)
        {
            var result = new BaseResponse<UploadFileResponse>();
            if (file != null)
            {
                var data = await _helper.UploadArticleFile(file);
                result.ReturnCode = ReturnCode.Success.Description();
                result.ReturnMessage = ReturnMessage.UploadSuccess.Description();
                result.Data = data;
            }
            else
            {
                result.ReturnCode = ReturnCode.Fail.Description();
                result.ReturnMessage = ReturnMessage.UploadFail.Description();
            }
            return result;
        }

        /// <summary>
        /// 檔案上傳到FireBase
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<BaseResponse<UploadFileResponse>> UploadProjectExperienceFile(IFormFile file)
        {
            var result = new BaseResponse<UploadFileResponse>();
            if (file != null)
            {
                var data = await _helper.UploadProjectExperienceFile(file);
                result.ReturnCode = ReturnCode.Success.Description();
                result.ReturnMessage = ReturnMessage.UploadSuccess.Description();
                result.Data = data;
            }
            else
            {
                result.ReturnCode = ReturnCode.Fail.Description();
                result.ReturnMessage = ReturnMessage.UploadFail.Description();
            }
            return result;
        }
    }
}

