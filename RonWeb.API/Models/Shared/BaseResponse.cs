using System.ComponentModel;

namespace RonWeb.API.Models.Shared
{
    public class BaseResponse<T>: BaseResponse
    {
        public T Data { get; set; }
    }

    public class BaseResponse
    {
        /// <summary>
        /// 回傳代碼
        /// </summary>
        public string ReturnCode { get; set; } = string.Empty;

        /// <summary>
        /// 回傳訊息
        /// </summary>
        public string ReturnMessage { get; set; } = string.Empty;
    }

    public enum ReturnCode
    {
        [Description("00")]
        Success,
        [Description("96")]
        AuthExpired,
        [Description("97")]
        Unique,
        [Description("98")]
        NotFound,
        [Description("99")]
        Fail
    }

    public enum ReturnMessage
    {
        [Description("取得資料成功")]
        Success,
        [Description("取得資料失敗")]
        Fail,
        [Description("新增資料成功")]
        CreateSuccess,
        [Description("新增資料失敗")]
        CreateFail,
        [Description("修改資料成功")]
        ModifySuccess,
        [Description("修改資料失敗")]
        ModifyFail,
        [Description("刪除資料成功")]
        DeleteSuccess,
        [Description("刪除資料失敗")]
        DeleteFail,
        [Description("登入成功")]
        LoginSuccess,
        [Description("帳號或密碼錯誤")]
        LoginFail,
        [Description("找不到資料")]
        NotFound,
        [Description("已有重複資料")]
        Unique,
        [Description("系統發生錯誤")]
        SystemFail,
        [Description("身分驗證過期")]
        AuthExpired,
        [Description("身分驗證失敗")]
        AuthFail,
        [Description("寄送信件成功")]
        SendMailSuccess,
        [Description("寄送信件失敗")]
        SendMailFail,
        [Description("上傳成功")]
        UploadSuccess,
        [Description("上傳失敗")]
        UploadFail
    }
}
