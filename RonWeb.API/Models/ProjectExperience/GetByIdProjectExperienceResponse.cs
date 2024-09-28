using RonWeb.API.Models.Shared;

namespace RonWeb.API.Models.ProjectExperience
{
    public class GetByIdProjectExperienceResponse
    {
        /// <summary>
        /// 專案經歷Id
        /// </summary>
        public int ProjectExperienceId { get; set; }

        /// <summary>
        /// 專案名稱
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// 專案描述
        /// </summary>
        public string Description { get; set; } = null!;

        /// <summary>
        /// 專案貢獻
        /// </summary>
        public string Contributions { get; set; } = null!;

        /// <summary>
        /// 專案角色
        /// </summary>
        public List<SelectItem<string>> ProjectRoles { get; set; } = new List<SelectItem<string>>();

        /// <summary>
        /// 專案技術
        /// </summary>
        public List<SelectItem<string>> TechnologyTools { get; set; } = new List<SelectItem<string>>();

        /// <summary>
        /// 創建日期
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 創建人員
        /// </summary>
        public long CreateBy { get; set; }

        /// <summary>
        /// 修改日期
        /// </summary>
        public DateTime? UpdateDate { get; set; }

        /// <summary>
        /// 修改人員
        /// </summary>
        public long? UpdateBy { get; set; }
    }
}
