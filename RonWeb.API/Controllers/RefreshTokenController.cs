using Microsoft.AspNetCore.Mvc;
using RonWeb.API.Interface.RefreshToken;
using RonWeb.API.Interface.Shared;
using RonWeb.API.Models.CustomizeException;
using RonWeb.API.Models.RefreshToken;
using RonWeb.API.Models.Shared;
using RonWeb.Core;

namespace RonWeb.API.Controllers
{
    /// <summary>
    /// 刷新接口
    /// </summary>
    [Route("api/[controller]")]
    public class RefreshTokenController : Controller
    {
        private readonly IRefreshTokenHelper _helper;
        private readonly ILogHelper _logger;
        public RefreshTokenController(IRefreshTokenHelper helper, ILogHelper logger)
        {
            _helper = helper;
            _logger = logger;
        }

        /// <summary>
        /// 刷新Token
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<BaseResponse<Token>> RefreshToken([FromBody] RefreshTokenRequest req)
        {
            var result = new BaseResponse<Token>();
            var data = await _helper.Refresh(req);
            result.ReturnCode = ReturnCode.Success.Description();
            result.ReturnMessage = ReturnMessage.LoginSuccess.Description();
            result.Data = data;
            return result;
        }
    }
}

