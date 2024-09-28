using Microsoft.AspNetCore.Mvc;
using RonWeb.API.Interface.Login;
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
            _helper = helper;
        }

        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<BaseResponse<LoginResponse>> Login([FromBody] LoginRequest req)
        {
            var result = new BaseResponse<LoginResponse>();
            var data = await _helper.Login(req);
            result.ReturnCode = ReturnCode.Success.Description();
            result.ReturnMessage = ReturnMessage.LoginSuccess.Description();
            result.Data = data;
            return result;
        }
    }
}

