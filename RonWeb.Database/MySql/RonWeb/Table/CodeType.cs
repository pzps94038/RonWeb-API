using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RonWeb.Database.MySql.RonWeb.Table
{
    public class CodeType
    {
        /// <summary>
        /// 類型代碼
        /// </summary>
        [Key]
        [Required]
        public string CodeTypeId { get; set; } = string.Empty;
        /// <summary>
        /// 類型名稱
        /// </summary>

        [Required]
        public string CodeTypeName { get; set; } = string.Empty;

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
        /// 關聯多個Code
        /// </summary>
        public virtual ICollection<Code> Codes { get; set; } = new List<Code>();
    }
}
