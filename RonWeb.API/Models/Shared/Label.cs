using System;
namespace RonWeb.API.Models.Shared
{
    public class Label
    {
        /// <summary>
        /// 標籤ID
        /// </summary>
        public long? LabelId { get; set; }
        /// <summary>
        /// 標籤名稱
        /// </summary>
        public string? LabelName { get; set; } = string.Empty;
        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime? CreateDate { get; set; }
    }
}

