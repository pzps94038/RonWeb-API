using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RonWeb.Database.MySql.RonWeb.Table
{
    /// <summary>
    /// User表
    /// </summary>
    public class UserMain
    {
        /// <summary>
        /// UserId
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long UserId { get; set; }

        /// <summary>
        /// 帳號
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Account { get; set; } = string.Empty;

        /// <summary>
        /// 密碼
        /// </summary>
        [StringLength(250)]
        [Required]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// 姓名
        /// </summary>
        [Required]
        [StringLength(20)]
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// 電子郵件
        /// </summary>
        [StringLength(250)]
        [Required]
        public string? Email { get; set; }

        /// <summary>
        /// 建立日期
        /// </summary>
        [Required]
        public DateTime CreateDate { get; set;}

        /// <summary>
        /// 更新日期
        /// </summary>
        public DateTime? UpdateDate { get; set; }
    }
}