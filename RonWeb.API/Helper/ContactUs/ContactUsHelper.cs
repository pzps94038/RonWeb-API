using System;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;
using Microsoft.AspNetCore.DataProtection;
using Newtonsoft.Json;
using RonWeb.API.Enum;
using RonWeb.API.Helper.Shared;
using RonWeb.API.Interface.ContactUs;
using RonWeb.API.Models.ContactUs;
using RonWeb.API.Models.CustomizeException;
using RonWeb.API.Models.Shared;
using RonWeb.Core;
using RonWeb.Database.Models;

namespace RonWeb.API.Helper.ContactUs
{
    public class ContactUsHelper : IContactUsHelper
    {
        public async Task SendContactUsMail(ContactUsRequest data)
        {
            var client = new HttpClient();
            var serverToken = Environment.GetEnvironmentVariable(EnvVarEnum.RE_CAPTCHA_SERVER_TOKEN.Description())!;
            var url = @$"https://www.google.com/recaptcha/api/siteverify?secret={serverToken}&response={data.ClientToken}";
            var buffer = Encoding.UTF8.GetBytes("");
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var res = await client.PostAsync(url, byteContent);
            string result = await res.Content.ReadAsStringAsync();
            var recapcha = JsonConvert.DeserializeObject<ReCAPTCHA>(result)!;
            if (recapcha.Success && recapcha.Score > 5)
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
            else
            {
                throw new AuthFailException();
            }
        }
    }
}

