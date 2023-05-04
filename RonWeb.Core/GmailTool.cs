using System;
using System.Net.Mail;

namespace RonWeb.Core
{
    public class GMail
    {
        /// <summary>
        /// 收件人 Email 地址
        /// </summary>
        public List<string> Emails { get; set; } = new List<string>();

        /// <summary>
        /// 主旨
        /// </summary>
        public string Subject = string.Empty;

        /// <summary>
        /// 內容
        /// </summary>
        public string Body = string.Empty;

        /// <summary>
        /// 內文是否為 HTML
        /// </summary>
        public bool IsBodyHtml = true;

        /// <summary>
        /// 優先權
        /// </summary>
        public MailPriority Priority = MailPriority.Normal;

        /// <summary>
        /// 附加檔案，所有對應檔案的路徑
        /// </summary>
        public List<string> AttachmentPaths = new List<string>();

        /// <summary>
        /// 發信來源,最好與你發送信箱相同,否則容易被其他的信箱判定為垃圾郵件.
        /// </summary>
        public MailAddress From;

        public string GmailSmtPwd { get; set; }

        public GMail(string address, string displayName, string pwd)
        {
            this.From = new MailAddress(address, displayName);
            this.GmailSmtPwd = pwd;
        }
    }

	public class GmailTool
	{
        public void SendMail(GMail gmail)
        {
            var mail = new MailMessage();

            // 收件人 Email 地址
            foreach (var email in gmail.Emails)
            {
                mail.To.Add(email);
            }
            // 主旨
            mail.Subject = gmail.Subject;
            // 內文
            mail.Body = gmail.Body;
            // 內文是否為 HTML
            mail.IsBodyHtml = gmail.IsBodyHtml;
            // 優先權
            mail.Priority = gmail.Priority;

            // 發信來源,最好與你發送信箱相同,否則容易被其他的信箱判定為垃圾郵件.
            mail.From = gmail.From;

            // 附加檔案,如果沒有附加檔案不用這一趴
            foreach (var path in gmail.AttachmentPaths)
            {
                Attachment attachment = new Attachment(path);
                mail.Attachments.Add(attachment);
            }

            // Gmail 的 SMTP 設定
            var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new System.Net.NetworkCredential("SenderEmail", gmail.GmailSmtPwd),
                EnableSsl = true,
                UseDefaultCredentials = false,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            // 投遞出去
            smtp.Send(mail);

            mail.Dispose();
        }
    }
}

