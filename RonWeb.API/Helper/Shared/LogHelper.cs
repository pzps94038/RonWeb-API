using System;
using Microsoft.IdentityModel.Logging;
using RonWeb.API.Controllers;
using RonWeb.API.Enum;
using RonWeb.API.Interface.ArticleCategory;
using RonWeb.API.Interface.Shared;
using RonWeb.Core;
using RonWeb.Database.MySql.RonWeb.DataBase;
using RonWeb.Database.MySql.RonWeb.Table;
using RonWeb.Database.Service;

namespace RonWeb.API.Helper.Shared
{
	public enum Level
	{
		Info,
        Warn,
		Error,
	}

	public class LogHelper: ILogHelper
    {
        private readonly ILogger<LogHelper> _logger;
        public LogHelper(ILogger<LogHelper> logger)
        {
            this._logger = logger;
        }

        public void Warn(string msg)
        {
            try
            {
                _logger.LogWarning(msg);
            }
            catch (Exception e)
            {

            }
        }

        public async void Error(Exception ex)
        {
            try
            {
                _logger.LogError(ex, "發生例外: {Message}", ex.Message);
                var tool = new GmailTool();
                var gmailAddress = Environment.GetEnvironmentVariable(EnvVarEnum.GMAIL_ADDRESS.Description())!;
                var gmailDisplayName = Environment.GetEnvironmentVariable(EnvVarEnum.GMAIL_DISPLAY_NAME.Description())!;
                var senderMail = Environment.GetEnvironmentVariable(EnvVarEnum.GMAIL_SENDER_EMAIL.Description())!;
                var gmailPwd = Environment.GetEnvironmentVariable(EnvVarEnum.GMAIL_PWD.Description())!;
                var errorLogAddress = Environment.GetEnvironmentVariable(EnvVarEnum.ERROR_LOG_EMAIL_ADDRESS.Description())!.Split(',').ToList();
                var mail = new GMail(gmailAddress, gmailDisplayName, senderMail, gmailPwd);
                mail.Emails = errorLogAddress;
                mail.Subject = "RonWeb-系統發生異常";
                mail.Body = @$"<h3>系統發生異常</h3>
                            <h4>異常訊息: {ex.Message}</br></h4>
                            <h4>異常時間: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}</br></h4>
                            <p>異常Stack: {ex.StackTrace}</p>
                        ";
                mail.Priority = System.Net.Mail.MailPriority.High;
                await tool.SendMail(mail);
            }
            catch (Exception e)
            {

            }
        }
    }
}

