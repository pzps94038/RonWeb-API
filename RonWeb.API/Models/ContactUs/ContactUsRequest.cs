using System;
namespace RonWeb.API.Models.ContactUs
{
	public class ContactUsRequest
	{
		/// <summary>
		/// Google Toke
		/// </summary>
		public string ClientToken { get; set; } = string.Empty;

		/// <summary>
		/// 信件主旨
		/// </summary>
		public string Subject { get; set; } = string.Empty;

		/// <summary>
		/// Email
		/// </summary>
        public string Email { get; set; } = string.Empty;

		/// <summary>
		/// 信件內容
		/// </summary>
        public string Body { get; set; } = string.Empty;
    }
}

