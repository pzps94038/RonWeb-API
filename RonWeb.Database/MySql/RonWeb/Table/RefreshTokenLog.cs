using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RonWeb.Database.MySql.RonWeb.Table
{
    /// <summary>
    /// 刷新Token
    /// </summary>
    public class RefreshTokenLog
    {

        /// <summary>
        /// RefreshToken
        /// </summary>
        [Key]
        public string RefreshToken { get; set; } = string.Empty;

        /// <summary>
        /// UserId
        /// </summary>
        [Key]
        public long UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual UserMain? UserMain { get; set; }

        /// <summary>
        /// 過期時間
        /// </summary>
        [Required]
        public DateTime ExpirationDate { get; set; }

        /// <summary>
        /// 建立日期
        /// </summary>
        [Required]
        public DateTime CreateDate { get; set; }
    }
}
