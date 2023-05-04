using System;
namespace RonWeb.API.Models.Shared
{
	public class ReCAPTCHA
	{
		public bool Success { get; set; } = false;
		public int Score { get; set; } = 0;
		public string Action { get; set; } = string.Empty;
		public DateTime Challenge_ts;
		public string Hostname { get; set; } = string.Empty;
	}
}

