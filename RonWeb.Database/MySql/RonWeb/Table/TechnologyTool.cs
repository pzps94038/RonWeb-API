using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RonWeb.Database.MySql.RonWeb.Table
{
    public class TechnologyTool
    {
        /// <summary>
        /// 專案經歷Id
        /// </summary>
        [Required]
        [Key]
        public int ProjectExperienceId { get; set; }

        [ForeignKey("ProjectExperienceId")]
        public virtual ProjectExperience? ProjectExperience { get; set; }

        /// <summary>
        /// TechnologyToolId
        /// </summary>
        [Required]
        [Key]
        public string TechnologyToolId { get; set; }

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
