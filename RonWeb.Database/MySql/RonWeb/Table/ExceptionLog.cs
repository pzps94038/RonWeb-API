using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RonWeb.Database.MySql.RonWeb.Table
{
    /// <summary>
    /// 意外Log
    /// </summary>
    public class ExceptionLog
    {
        /// <summary>
        /// LogId
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LogId { get; set; }

        /// <summary>
        /// 訊息
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// 堆疊
        /// </summary>
        public string? StackTrace { get; set; }

        /// <summary>
        /// 信息級別
        /// </summary>
        public string Level { get; set; } = string.Empty;

        /// <summary>
        /// 建立日期
        /// </summary>
        [Required]
        public DateTime CreateDate { get; set; }
    }
}
