using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RonWeb.API.Helper.Shared;
using RonWeb.API.Interface.Search;
using RonWeb.API.Interface.Upload;
using RonWeb.API.Models.Shared;
using RonWeb.Core;
using static System.Runtime.InteropServices.JavaScript.JSType;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RonWeb.API.Controllers
{
    /// <summary>
    /// 檔案上傳
    /// </summary>
    [Route("api/[controller]")]
    public class UploadController : Controller
    {
        private readonly IUploadHelper _helper;
        public UploadController(IUploadHelper helper)
        {
            this._helper = helper;
        }

        /// <summary>
        /// 檔案上傳到FireBase
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<FileUploadResponse> UploadFile()
        {
            var result = new FileUploadResponse();

            try
            {
                var file = HttpContext.Request.Form.Files.FirstOrDefault();
                if (file != null)
                {
                    var url = await this._helper.UploadFile(file);
                    result.ReturnCode = ReturnCode.Success.Description();
                    result.ReturnMessage = ReturnMessage.UploadSuccess.Description();
                    result.Url = url;
                }
                else
                {
                    result.ReturnCode = ReturnCode.Fail.Description();
                    result.ReturnMessage = ReturnMessage.UploadFail.Description();
                }
                
            }
            catch (Exception ex)
            {
                result.ReturnCode = ReturnCode.Fail.Description();
                result.ReturnMessage = ReturnMessage.UploadFail.Description();
                MongoLogHelper.Error(ex);
            }

            return result;
        }
    }
}

