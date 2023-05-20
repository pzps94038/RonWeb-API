using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RonWeb.Database.MySql.RonWeb.Table
{
    /// <summary>
    /// 文章標籤
    /// </summary>
    public class ArticleLabel
    {
        /// <summary>
        /// 標籤 Id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LabelId { get; set; }

        /// <summary>
        /// 標籤名稱
        /// </summary>
        [Required]
        [StringLength(20)]
        public string LabelName { get; set; } = string.Empty;

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

        public static explicit operator ArticleLabel(Task<ArticleLabel?> v)
        {
            throw new NotImplementedException();
        }
    }
}
