using System;
using RonWeb.API.Models.ProjectExperience;

namespace RonWeb.API.Interface.AdminProjectExperience
{
    public interface IAdminProjectExperienceGet
    {
        public Task<GetByIdProjectExperienceResponse> GetAsync(long id);
    }
}