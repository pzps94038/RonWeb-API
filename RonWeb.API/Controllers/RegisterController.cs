using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RonWeb.API.Interface.Register;
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
        public RegisterController(IRegisterHelper helper)
        {
            _helper = helper;
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
            await _helper.RegisterUser(data);
            result.ReturnCode = ReturnCode.Success.Description();
            result.ReturnMessage = ReturnMessage.CreateSuccess.Description();
            return result;
        }

    }
}

