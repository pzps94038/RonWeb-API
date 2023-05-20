using System;
using RonWeb.API.Enum;
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

	public static class LogHelper
    {
		public static async void Info(string msg)
		{
            using (var db = new RonWebDbContext())
            {
                var data = new ExceptionLog()
                {
                    Message = msg,
                    Level = Level.Info.Description(),
                    CreateDate = DateTime.Now
                };
                await db.ExceptionLog.AddAsync(data);
                await db.SaveChangesAsync();
            }
        }

        public static async void Warn(string msg)
        {
            using (var db = new RonWebDbContext())
            {
                var data = new ExceptionLog()
                {
                    Message = msg,
                    Level = Level.Warn.Description(),
                    CreateDate = DateTime.Now
                };
                await db.ExceptionLog.AddAsync(data);
                await db.SaveChangesAsync();
            }
        }

        public static async void Warn(Exception ex)
        {
            using (var db = new RonWebDbContext())
            {
                var data = new ExceptionLog()
                {
                    StackTrace = ex.StackTrace,
                    Message = ex.Message,
                    Level = Level.Warn.Description(),
                    CreateDate = DateTime.Now
                };
                await db.ExceptionLog.AddAsync(data);
                await db.SaveChangesAsync();
            }
        }

        public static async void Error(Exception ex, bool record = true)
        {
            try
            {
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
                if (record)
                {
                    using (var db = new RonWebDbContext())
                    {
                        var data = new ExceptionLog()
                        {
                            StackTrace = ex.StackTrace,
                            Message = ex.Message,
                            Level = Level.Error.Description(),
                            CreateDate = DateTime.Now
                        };
                        await db.ExceptionLog.AddAsync(data);
                        await db.SaveChangesAsync();
                    }
                }
            }
            catch (Exception e)
            {

            }
        }
    }
}

