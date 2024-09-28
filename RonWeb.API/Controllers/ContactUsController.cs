using Microsoft.AspNetCore.Mvc;
using RonWeb.API.Interface.ContactUs;

namespace RonWeb.API.Controllers
{
    /// <summary>
    /// 聯絡我們
    /// </summary>
    [Route("api/[controller]")]
    public class ContactUsController : Controller
    {
        private readonly IContactUsHelper _helper;
        public ContactUsController(IContactUsHelper helper)
        {
            _helper = helper;
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
        //        await _helper.SendContactUsMail(req);
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

