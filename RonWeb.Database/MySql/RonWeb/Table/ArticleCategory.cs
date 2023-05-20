using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RonWeb.Database.MySql.RonWeb.Table
{
    /// <summary>
    /// 文章分類
    /// </summary>
    public class ArticleCategory
    {
        /// <summary>
        /// 分類 Id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CategoryId { get; set; }

        /// <summary>
        /// 類別名稱
        /// </summary>
        [Required]
        [StringLength(20)]
        public string CategoryName = string.Empty;

        /// <summary>
        /// 建立日期
        /// </summary>
        [Required]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 建立者
        /// </summary>
        [Required]
        public long CreateBy { get; set; }

        /// <summary>
        /// 更新日期
        /// </summary>
        public DateTime? UpdateDate { get; set; }

        /// <summary>
        /// 更新者
        /// </summary>
        public long? UpdateBy { get; set; }
    }
}
