using Microsoft.AspNetCore.Mvc;
using RonWeb.API.Filter;
using RonWeb.API.Interface.ContactUs;
using RonWeb.API.Interface.Shared;
using RonWeb.API.Models.ContactUs;
using RonWeb.API.Models.CustomizeException;
using RonWeb.API.Models.Shared;
using RonWeb.Core;

namespace RonWeb.API.Controllers
{
    /// <summary>
    /// 聯絡我們
    /// </summary>
    [Route("api/[controller]")]
    [ServiceFilter(typeof(HostFilter))]
    public class ContactUsController : Controller
    {
        private readonly IContactUsHelper _helper;
        private readonly ILogHelper _logger;
        public ContactUsController(IContactUsHelper helper, ILogHelper logger)
        {
            this._helper = helper;
            this._logger = logger;
        }

        ///// <summary>
        ///// 聯絡資訊新增
        ///// </summary>
        ///// <param name="req"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public async Task<BaseResponse> ContactUs([FromBody] ContactUsRequest req)
        //{
        //    var result = new BaseResponse();
        //    try
        //    {
        //        await this._helper.SendContactUsMail(req);
        //        result.ReturnCode = ReturnCode.Success.Description();
        //        result.ReturnMessage = ReturnMessage.SendMailSuccess.Description();
        //    }
        //    catch (AuthFailException)
        //    {
        //        result.ReturnCode = ReturnCode.Fail.Description();
        //        result.ReturnMessage = ReturnMessage.AuthFail.Description();
        //    }
        //    catch (Exception ex)
        //    {
        //        result.ReturnCode = ReturnCode.Fail.Description();
        //        result.ReturnMessage = ReturnMessage.SendMailFail.Description();
        //        _logger.Error(ex);
        //    }
        //    return result;
        //}
    }
}

