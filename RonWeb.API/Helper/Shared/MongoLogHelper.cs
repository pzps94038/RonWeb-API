using System;
using RonWeb.API.Enum;
using RonWeb.API.Interface.Shared;
using RonWeb.Core;
using RonWeb.Database.Models;
using RonWeb.Database.Mongo;
using RonWeb.Database.Service;

namespace RonWeb.API.Helper.Shared
{
	public enum Level
	{
		Info,
        Warn,
		Error,
	}

	public static class MongoLogHelper
    {
		public static async void Info(string msg)
		{
            string conStr = Environment.GetEnvironmentVariable(EnvVarEnum.RON_WEB_MONGO_DB_CONSTR.Description())!;
            var srv = new MongoDbService(conStr, MongoDbEnum.RonWeb.Description());
            var data = new ExceptionLog()
            {
                Message = msg,
				Level = Level.Info.Description(),
				CreateDate = DateTime.Now
			};
			await srv.CreateAsync<ExceptionLog>(data);
        }

        public static async void Warn(string msg)
        {
            string conStr = Environment.GetEnvironmentVariable(EnvVarEnum.RON_WEB_MONGO_DB_CONSTR.Description())!;
            var srv = new MongoDbService(conStr, MongoDbEnum.RonWeb.Description());
            var data = new ExceptionLog()
            {
                Message = msg,
                Level = Level.Warn.Description(),
                CreateDate = DateTime.Now
            };
            await srv.CreateAsync<ExceptionLog>(data);
        }

        public static async void Warn(Exception ex)
        {
            string conStr = Environment.GetEnvironmentVariable(EnvVarEnum.RON_WEB_MONGO_DB_CONSTR.Description())!;
            var srv = new MongoDbService(conStr, MongoDbEnum.RonWeb.Description());
            var data = new ExceptionLog()
            {
                StackTrace = ex.StackTrace,
                Message = ex.Message,
                Level = Level.Warn.Description(),
                CreateDate = DateTime.Now
            };
            await srv.CreateAsync<ExceptionLog>(data);
        }

        public static async void Error(Exception ex)
        {
            try
            {
                string conStr = Environment.GetEnvironmentVariable(EnvVarEnum.RON_WEB_MONGO_DB_CONSTR.Description())!;
                var srv = new MongoDbService(conStr, MongoDbEnum.RonWeb.Description());
                var data = new ExceptionLog()
                {
                    StackTrace = ex.StackTrace,
                    Message = ex.Message,
                    Level = Level.Error.Description(),
                    CreateDate = DateTime.Now
                };
                await srv.CreateAsync<ExceptionLog>(data);
                var tool = new GmailTool();
                var gmailAddress = Environment.GetEnvironmentVariable(EnvVarEnum.GMAIL_ADDRESS.Description())!;
                var gmailDisplayName = Environment.GetEnvironmentVariable(EnvVarEnum.GMAIL_DISPLAY_NAME.Description())!;
                var senderMail = Environment.GetEnvironmentVariable(EnvVarEnum.GMAIL_SENDER_EMAIL.Description())!;
                var gmailPwd = Environment.GetEnvironmentVariable(EnvVarEnum.GMAIL_PWD.Description())!;
                var errorLogAddress = Environment.GetEnvironmentVariable(EnvVarEnum.ERROR_LOG_EMAIL_ADDRESS.Description())!.Split(',').ToList();
                try
                {
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
                catch (Exception mailEx)
                {
                    var mailExLog = new ExceptionLog()
                    {
                        StackTrace = mailEx.StackTrace,
                        Message = mailEx.Message,
                        Level = Level.Error.Description(),
                        CreateDate = DateTime.Now
                    };
                    await srv.CreateAsync<ExceptionLog>(mailExLog);
                }
            }
            catch (Exception e)
            {

            }
        }
    }
}

