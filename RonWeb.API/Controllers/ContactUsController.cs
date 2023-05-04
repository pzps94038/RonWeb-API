using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RonWeb.API.Helper.Shared;
using RonWeb.API.Interface.ContactUs;
using RonWeb.API.Interface.RefreshToken;
using RonWeb.API.Models.ContactUs;
using RonWeb.API.Models.CustomizeException;
using RonWeb.API.Models.Shared;
using RonWeb.Core;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RonWeb.API.Controllers
{
    [Route("api/[controller]")]
    public class ContactUsController : Controller
    {
        private readonly IContactUsHelper _helper;
        public ContactUsController(IContactUsHelper helper)
        {
            this._helper = helper;
        }

        [HttpPost]
        public async Task<BaseResponse> Post([FromBody] ContactUsRequest req)
        {
            var result = new BaseResponse();
            try
            {
                await this._helper.SendContactUsMail(req);
                result.ReturnCode = ReturnCode.Success.Description();
                result.ReturnMessage = ReturnMessage.SendMailSuccess.Description();
            }
            catch (AuthFailException ex)
            {
                result.ReturnCode = ReturnCode.Fail.Description();
                result.ReturnMessage = ReturnMessage.AuthFail.Description();
            }
            catch (Exception ex)
            {
                result.ReturnCode = ReturnCode.Fail.Description();
                result.ReturnMessage = ReturnMessage.SendMailFail.Description();
                MongoLogHelper.Error(ex);
            }
            return result;
        }
    }
}

