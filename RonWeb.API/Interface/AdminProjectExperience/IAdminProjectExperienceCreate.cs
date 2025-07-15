using System;
using RonWeb.API.Models.ProjectExperience;

namespace RonWeb.API.Interface.AdminProjectExperience
{
    public interface IAdminProjectExperienceCreate
    {
        public Task CreateAsync(CreateProjectExperienceRequest data);
    }
}