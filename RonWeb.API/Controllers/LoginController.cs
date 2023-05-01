using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RonWeb.API.Helper.Shared;
using RonWeb.API.Interface.ArticleLabel;
using RonWeb.API.Interface.Login;
using RonWeb.API.Models.CustomizeException;
using RonWeb.API.Models.Login;
using RonWeb.API.Models.Shared;
using RonWeb.Core;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RonWeb.API.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private readonly ILoginHelper _helper;
        public LoginController(ILoginHelper helper)
        {
            this._helper = helper;
        }

        [HttpPost]
        public async Task<BaseResponse<Token>> Post([FromBody]LoginRequest req)
        {
            var result = new BaseResponse<Token>();
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

