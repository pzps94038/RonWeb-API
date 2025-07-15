using System;
using RonWeb.API.Models.ProjectExperience;

namespace RonWeb.API.Interface.AdminProjectExperience
{
    public interface IAdminProjectExperienceUpdate
    {
        public Task UpdateAsync(long id, UpdateProjectExperienceRequest data);
    }
}