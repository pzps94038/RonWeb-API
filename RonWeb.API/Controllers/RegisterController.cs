using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RonWeb.API.Interface.Register;
using RonWeb.API.Interface.Shared;
using RonWeb.API.Models.CustomizeException;
using RonWeb.API.Models.Register;
using RonWeb.API.Models.Shared;
using RonWeb.Core;

namespace RonWeb.API.Controllers
{
    /// <summary>
    /// 註冊
    /// </summary>
    [Route("api/[controller]")]
    [Authorize]
    public class RegisterController : Controller
    {
        private readonly IRegisterHelper _helper;
        private readonly ILogHelper _logger;
        public RegisterController(IRegisterHelper helper, ILogHelper logger)
        {
            this._helper = helper;
            this._logger = logger;
        }

        /// <summary>
        /// 註冊帳號
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<BaseResponse> Register([FromBody] RegisterRequest data)
        {
            var result = new BaseResponse();
            try
            {
                await this._helper.RegisterUser(data);
                result.ReturnCode = ReturnCode.Success.Description();
                result.ReturnMessage = ReturnMessage.CreateSuccess.Description();
            }
            catch (UniqueException)
            {
                result.ReturnCode = ReturnCode.Unique.Description();
                result.ReturnMessage = ReturnMessage.Unique.Description();
            }
            catch (Exception ex)
            {
                result.ReturnCode = ReturnCode.Fail.Description();
                result.ReturnMessage = ReturnMessage.SystemFail.Description();
                _logger.Error(ex);
            }
            return result;
        }

    }
}

