using System;
using RonWeb.API.Models.ProjectExperience;

namespace RonWeb.API.Interface.AdminProjectExperience
{
    public interface IAdminProjectExperienceHelper
    {
        public Task<GetProjectExperienceResponse> GetListAsync(int? page);
        public Task<GetByIdProjectExperienceResponse> GetAsync(long id);
        public Task CreateAsync(CreateProjectExperienceRequest data);
        public Task UpdateAsync(long id, UpdateProjectExperienceRequest data);
        public Task DeleteAsync(long data);
    }
}

