using Microsoft.AspNetCore.Mvc;
using RonWeb.API.Helper.Shared;
using RonWeb.API.Interface.Login;
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
        public LoginController(ILoginHelper helper)
        {
            this._helper = helper;
        }

        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<BaseResponse<LoginResponse>> Post([FromBody]LoginRequest req)
        {
            var result = new BaseResponse<LoginResponse>();
            try
            {
                var data = await this._helper.Login(req);
                result.ReturnCode = ReturnCode.Success.Description();
                result.ReturnMessage = ReturnMessage.LoginSuccess.Description();
                result.Data = data;
            }
            catch (NotFoundException ex)
            {
                result.ReturnCode = ReturnCode.Fail.Description();
                result.ReturnMessage = ReturnMessage.LoginFail.Description();
            }
            catch (Exception ex)
            {
                result.ReturnCode = ReturnCode.Fail.Description();
                result.ReturnMessage = ReturnMessage.SystemFail.Description();
                MongoLogHelper.Error(ex);
            }
            return result;
        }
    }
}

