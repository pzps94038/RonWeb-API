using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RonWeb.Database.MySql.RonWeb.Table
{
    public class Code
    {
        /// <summary>
        /// 代碼類型
        /// </summary>
        [Required]
        public string CodeTypeId { get; set; } = string.Empty;

        // 這是導航屬性，指向 CodeType 實體
        [ForeignKey("CodeTypeId")]
        public virtual CodeType? CodeType { get; set; }

        /// <summary>
        /// 代碼
        /// </summary>
        [Key]
        [Required]
        public string CodeId { get; set; } = string.Empty;

        /// <summary>
        /// 代碼名稱
        /// </summary>
        [Required]
        public string CodeName { get; set; } = string.Empty;

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
