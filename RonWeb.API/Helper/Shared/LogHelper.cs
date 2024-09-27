using RonWeb.API.Interface.Shared;
using RonWeb.Core;
using RonWeb.Database.Entities;

namespace RonWeb.API.Helper.Shared
{
    public enum Level
    {
        Info,
        Warn,
        Error,
    }

    public class LogHelper : ILogHelper
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public LogHelper(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public void Info(string msg)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<RonWebDbContext>();
                    var log = new ExceptionLog();
                    log.Message = msg;
                    log.Level = Level.Info.Description();
                    log.CreateDate = DateTime.Now;
                    db.ExceptionLog.Add(log);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void Warn(string msg)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<RonWebDbContext>();
                    var log = new ExceptionLog();
                    log.Message = msg;
                    log.Level = Level.Warn.Description();
                    log.CreateDate = DateTime.Now;
                    db.ExceptionLog.Add(log);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void Error(Exception ex)
        {
            try
            {
                Console.WriteLine("======================================================================");
                Console.WriteLine("錯誤訊息:" + ex.Message);
                Console.WriteLine("======================================================================");
                Console.WriteLine("錯誤Stack:" + ex.StackTrace);
                Console.WriteLine("======================================================================");
                if (ex.InnerException != null)
                {
                    Console.WriteLine("======================================================================");
                    Console.WriteLine("InnerException錯誤訊息:" + ex.InnerException.Message);
                    Console.WriteLine("======================================================================");
                    Console.WriteLine("InnerException錯誤Stack:" + ex.InnerException.StackTrace);
                    Console.WriteLine("======================================================================");
                }
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<RonWebDbContext>();
                    var log = new ExceptionLog();
                    log.Message = ex.Message;
                    log.StackTrace = ex.StackTrace;
                    log.Level = Level.Error.Description();
                    log.CreateDate = DateTime.Now;
                    db.ExceptionLog.Add(log);
                    db.SaveChanges();
                }
                //var tool = new GmailTool();
                //var gmailAddress = Environment.GetEnvironmentVariable(EnvVarEnum.GMAIL_ADDRESS.Description())!;
                //var gmailDisplayName = Environment.GetEnvironmentVariable(EnvVarEnum.GMAIL_DISPLAY_NAME.Description())!;
                //var senderMail = Environment.GetEnvironmentVariable(EnvVarEnum.GMAIL_SENDER_EMAIL.Description())!;
                //var gmailPwd = Environment.GetEnvironmentVariable(EnvVarEnum.GMAIL_PWD.Description())!;
                //var errorLogAddress = Environment.GetEnvironmentVariable(EnvVarEnum.ERROR_LOG_EMAIL_ADDRESS.Description())!.Split(',').ToList();
                //var mail = new GMail(gmailAddress, gmailDisplayName, senderMail, gmailPwd);
                //mail.Emails = errorLogAddress;
                //mail.Subject = "RonWeb-系統發生異常";
                //mail.Body = @$"<h3>系統發生異常</h3>
                //            <h4>異常訊息: {ex.Message}</br></h4>
                //            <h4>異常時間: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}</br></h4>
                //            <p>異常Stack: {ex.StackTrace}</p>
                //        ";
                //mail.Priority = System.Net.Mail.MailPriority.High;
                //await tool.SendMail(mail);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}

