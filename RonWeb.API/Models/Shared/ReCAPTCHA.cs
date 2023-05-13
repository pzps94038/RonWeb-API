using System;
namespace RonWeb.API.Models.Shared
{
	public class ReCAPTCHA
	{
		/// <summary>
		/// 是否成功
		/// </summary>
		public bool Success { get; set; } = false;
		/// <summary>
		/// 判別分數
		/// </summary>
		public double Score { get; set; } = 0;
		public string Action { get; set; } = string.Empty;
		public DateTime Challenge_ts;
		public string Hostname { get; set; } = string.Empty;
	}
}

