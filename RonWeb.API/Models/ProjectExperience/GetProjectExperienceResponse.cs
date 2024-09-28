using System;
using RonWeb.API.Models.Article;

namespace RonWeb.API.Models.ProjectExperience
{
    public class GetProjectExperienceResponse
    {
        /// <summary>
        /// 總數
        /// </summary>
        public int Total { get; set; } = 0;

        /// <summary>
        /// 專案經歷列表
        /// </summary>
        public List<ProjectExperienceItem> ProjectExperiences { get; set; } = new List<ProjectExperienceItem>();
    }
}

