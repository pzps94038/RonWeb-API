using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RonWeb.Database.MySql.RonWeb.Table
{
    /// <summary>
    /// 專案經歷
    /// </summary>
    public class ProjectExperience
    {
        /// <summary>
        /// 專案經歷Id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProjectExperienceId { get; set; }

        /// <summary>
        /// 專案名稱
        /// </summary>
        [Required]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 專案描述
        /// </summary>
        [Required]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 具體貢獻
        /// </summary>
        [Required]
        public string Contributions { get; set; } = string.Empty;

        /// <summary>
        /// 專案角色（多個角色）
        /// </summary>
        public virtual ICollection<ProjectRole> ProjectRole { get; set; } = new List<ProjectRole>();

        /// <summary>
        /// 技術工具（多個工具）
        /// </summary>
        public virtual ICollection<TechnologyTool> TechnologyTool { get; set; } = new List<TechnologyTool>();

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
