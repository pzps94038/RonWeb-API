using System;
namespace RonWeb.API.Models.ContactUs
{
	public class ContactUsRequest
	{
		public string ClientToken { get; set; } = string.Empty;
		public string Subject { get; set; } = string.Empty;
		public string Body { get; set; } = string.Empty;
    }
}

