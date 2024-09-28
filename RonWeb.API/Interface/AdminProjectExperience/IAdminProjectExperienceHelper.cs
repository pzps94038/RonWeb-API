using System;
using RonWeb.API.Interface.Shared;
using RonWeb.API.Models.Article;
using RonWeb.API.Models.ProjectExperience;

namespace RonWeb.API.Interface.AdminArticleHelper
{
    public interface IAdminProjectExperienceHelper : IGetAsync<long, GetByIdProjectExperienceResponse>,

        IDeleteAsync<long>,
        IUpdateAsync<long, UpdateProjectExperienceRequest>,
        ICreateAsync<CreateProjectExperienceRequest>
    {
        public Task<GetProjectExperienceResponse> GetListAsync(int? page);
    }
}

