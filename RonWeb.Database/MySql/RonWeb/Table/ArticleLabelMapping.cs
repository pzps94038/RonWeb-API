using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RonWeb.Database.MySql.RonWeb.Table
{
    /// <summary>
    /// 文章標籤對應
    /// </summary>
    public class ArticleLabelMapping
    {
        /// <summary>
        /// 標籤 Id
        /// </summary>
        [Key]
        [Required]
        public long LabelId { get; set; }

        /// <summary>
        /// 標籤表
        /// </summary>
        [ForeignKey("LabelId")]
        public virtual ArticleLabel ArticleLabel { get; set; }


        /// <summary>
        /// 文章 Id
        /// </summary>
        [Key]
        [Required]
        public long ArticleId { get; set; }

        /// <summary>
        /// 文章表
        /// </summary>
        [ForeignKey("ArticleId")]
        public virtual Article? Article { get; set; }

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
