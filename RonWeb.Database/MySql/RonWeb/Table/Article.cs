using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RonWeb.Database.MySql.RonWeb.Table
{
    /// <summary>
    /// 文章表
    /// </summary>
    public class Article
    {
        /// <summary>
        /// 文章 Id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ArticleId { get; set; }

        /// <summary>
        /// 文章標題
        /// </summary>
        [Required]
        public string ArticleTitle { get; set; } = string.Empty;

        /// <summary>
        /// 文章預覽內容
        /// </summary>
        [Required]
        public string PreviewContent { get; set; } = string.Empty;

        /// <summary>
        /// 文章內容
        /// </summary>
        [Required]
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// 類別Id
        /// </summary>
        [Required]
        public long CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual ArticleCategory ArticleCategory { get; set; }

        /// <summary>
        /// 瀏覽次數
        /// </summary>
        [Required]
        public int ViewCount { get; set; } = 0;

        /// <summary>
        /// 是否啟用
        /// </summary>
        [Required]
        [StringLength(1)]
        public string Flag { get; set; } = "Y";

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

        /// <summary>
        /// 關連標籤
        /// </summary>
        public ICollection<ArticleLabelMapping> ArticleLabelMapping { get; set; }

        /// <summary>
        /// 關連參考文章
        /// </summary>
        public ICollection<ArticleReferences> ArticleReferences { get; set; }
    }
}
