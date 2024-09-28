using RonWeb.API.Models.Shared;

namespace RonWeb.API.Models.ProjectExperience
{
    public class CreateProjectExperienceRequest
    {
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
        /// 專案描述上傳圖
        /// </summary>
        public List<UploadFile> DescriptionFiles { get; set; } = new List<UploadFile>();

        /// <summary>
        /// 專案貢獻上傳圖
        /// </summary>
        public List<UploadFile> ContributionsFiles { get; set; } = new List<UploadFile>();

        /// <summary>
        /// 專案角色
        /// </summary>
        public List<SelectItem<string>> ProjectRoles { get; set; } = new List<SelectItem<string>>();

        /// <summary>
        /// 使用技術
        /// </summary>
        public List<SelectItem<string>> TechnologyTools { get; set; } = new List<SelectItem<string>>();

        /// <summary>
        /// 創建人
        /// </summary>
        public long UserId { get; set; }
    }
}
