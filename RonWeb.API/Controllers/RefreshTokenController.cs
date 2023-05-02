using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RonWeb.API.Helper.Shared;
using RonWeb.API.Interface.Login;
using RonWeb.API.Interface.RefreshToken;
using RonWeb.API.Models.CustomizeException;
using RonWeb.API.Models.RefreshToken;
using RonWeb.API.Models.Shared;
using RonWeb.Core;

namespace RonWeb.API.Controllers
{
    [Route("api/[controller]")]
    public class RefreshTokenController : Controller
    {
        private readonly IRefreshTokenHelper _helper;
        public RefreshTokenController(IRefreshTokenHelper helper)
        {
            this._helper = helper;
        }

        [HttpPost]
        public async Task<BaseResponse<Token>> Post([FromBody]RefreshTokenRequest req)
        {
            var result = new BaseResponse<Token>();
            try
            {
                var data = await this._helper.Refresh(req);
                result.ReturnCode = ReturnCode.Success.Description();
                result.ReturnMessage = ReturnMessage.LoginSuccess.Description();
                result.Data = data;
            }
            catch (NotFoundException ex)
            {
                result.ReturnCode = ReturnCode.Fail.Description();
                result.ReturnMessage = ReturnMessage.NotFound.Description();
            }
            catch (AuthExpiredException ex) 
            {
                result.ReturnCode = ReturnCode.AuthExpired.Description();
                result.ReturnMessage = ReturnMessage.AuthExpired.Description();
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

