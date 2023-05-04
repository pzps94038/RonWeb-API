using System;
using RonWeb.API.Enum;
using RonWeb.API.Helper.Shared;
using RonWeb.API.Interface.ContactUs;
using RonWeb.API.Models.ContactUs;
using RonWeb.Core;
using RonWeb.Database.Models;

namespace RonWeb.API.Helper.ContactUs
{
    public class ContactUsHelper : IContactUsHelper
    {
        public async Task SendContactUsMail(ContactUsRequest data)
        {
            var gmailAddress = Environment.GetEnvironmentVariable(EnvVarEnum.GMAIL_ADDRESS.Description())!;
            var gmailDisplayName = Environment.GetEnvironmentVariable(EnvVarEnum.GMAIL_DISPLAY_NAME.Description())!;
            var senderMail = Environment.GetEnvironmentVariable(EnvVarEnum.GMAIL_SENDER_EMAIL.Description())!;
            var gmailPwd = Environment.GetEnvironmentVariable(EnvVarEnum.GMAIL_PWD.Description())!;
            var errorLogAddress = Environment.GetEnvironmentVariable(EnvVarEnum.ERROR_LOG_EMAIL_ADDRESS.Description())!.Split(',').ToList();
            var mail = new GMail(gmailAddress, gmailDisplayName, senderMail, gmailPwd);
            mail.Emails = errorLogAddress;
            mail.Subject = data.Subject;
            mail.Body = data.Body;
            mail.Priority = System.Net.Mail.MailPriority.Normal;
            var tool = new GmailTool();
            await tool.SendMail(mail);
        }
    }
}

