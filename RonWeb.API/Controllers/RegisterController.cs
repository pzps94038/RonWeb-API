using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RonWeb.API.Helper.Shared;
using RonWeb.API.Interface.Register;
using RonWeb.API.Interface.Search;
using RonWeb.API.Models.CustomizeException;
using RonWeb.API.Models.Register;
using RonWeb.API.Models.Shared;
using RonWeb.Core;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RonWeb.API.Controllers
{
    [Route("api/[controller]")]
    public class RegisterController : Controller
    {
        private readonly IRegisterHelper _helper;
        public RegisterController(IRegisterHelper helper)
        {
            this._helper = helper;
        }
        [HttpPost]
        public async Task<BaseResponse> Post([FromBody]RegisterRequest data)
        {
            var result = new BaseResponse();
            try
            {
                await this._helper.RegisterUser(data);
                result.ReturnCode = ReturnCode.Success.Description();
                result.ReturnMessage = ReturnMessage.CreateSuccess.Description();
            }
            catch (UniqueException ex)
            {
                result.ReturnCode = ReturnCode.Unique.Description();
                result.ReturnMessage = ReturnMessage.Unique.Description();
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

