using System;
using RonWeb.API.Models.ProjectExperience;

namespace RonWeb.API.Interface.AdminProjectExperience
{
    public interface IAdminProjectExperienceHelper : IAdminProjectExperienceGet,
        IAdminProjectExperienceDelete,
        IAdminProjectExperienceUpdate,
        IAdminProjectExperienceCreate
    {
        public Task<GetProjectExperienceResponse> GetListAsync(int? page);
    }
}

