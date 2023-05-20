using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RonWeb.Database.MySql.RonWeb.Table
{
    /// <summary>
    /// 文章預覽內容圖片
    /// </summary>
    public class ArticlePrevImage
    {
        /// <summary>
        /// Image Id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ImageId { get; set; }

        /// <summary>
        /// 文章Id
        /// </summary>
        [Required]
        public long ArticleId { get; set; }

        [ForeignKey("ArticleId")]
        public virtual Article? Article { get; set; }

        /// <summary>
        /// 檔名
        /// </summary>
        [StringLength(30)]
        public string FileName { get; set; } = string.Empty;

        /// <summary>
        /// 檔案路徑
        /// </summary>
        public string Path { get; set; } = string.Empty;

        /// <summary>
        /// Network Path
        /// </summary>
        public string Url { get; set; } = string.Empty;

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
