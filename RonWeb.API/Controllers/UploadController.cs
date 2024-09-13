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
    [Route("api/[controller]")]
    [Authorize]
    public class UploadController : Controller
    {
        private readonly IUploadHelper _helper;
        private readonly ILogHelper _logger;
        public UploadController(IUploadHelper helper, ILogHelper logger)
        {
            _helper = helper;
            _logger = logger;
        }

        /// <summary>
        /// 檔案上傳到FireBase
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<BaseResponse<UploadFileResponse>> UploadFile(IFormFile file)
        {
            var result = new BaseResponse<UploadFileResponse>();
            if (file != null)
            {
                var data = await _helper.UploadFile(file);
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

