using Microsoft.AspNetCore.Mvc;
using RonWeb.API.Helper.Shared;
using RonWeb.API.Interface.Login;
using RonWeb.API.Interface.Shared;
using RonWeb.API.Models.CustomizeException;
using RonWeb.API.Models.Login;
using RonWeb.API.Models.Shared;
using RonWeb.Core;

namespace RonWeb.API.Controllers
{
    /// <summary>
    /// 登入
    /// </summary>
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private readonly ILoginHelper _helper;
        private readonly ILogHelper _logger;
        public LoginController(ILoginHelper helper, ILogHelper logger)
        {
            this._helper = helper;
            this._logger = logger;
        }

        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<BaseResponse<LoginResponse>> Login([FromBody]LoginRequest req)
        {
            var result = new BaseResponse<LoginResponse>();
            try
            {
                var data = await this._helper.Login(req);
                result.ReturnCode = ReturnCode.Success.Description();
                result.ReturnMessage = ReturnMessage.LoginSuccess.Description();
                result.Data = data;
            }
            catch (NotFoundException)
            {
                result.ReturnCode = ReturnCode.Fail.Description();
                result.ReturnMessage = ReturnMessage.LoginFail.Description();
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

