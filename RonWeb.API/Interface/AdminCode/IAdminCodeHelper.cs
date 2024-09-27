using System;
using RonWeb.API.Interface.Shared;
using RonWeb.API.Models.Article;
using RonWeb.API.Models.ArticleCategory;
using RonWeb.API.Models.ArticleLabel;
using RonWeb.API.Models.CodeType;
using RonWeb.Database.Entities;

namespace RonWeb.API.Interface.AdminArticleHelper
{
    public interface IAdminCodeHelper :
        IGetAsync<long, Code>,
        ICreateAsync<CreateCodeRequest>,
        IUpdateAsync<long, UpdateCodeRequest>,
        IDeleteAsync<long>
    {
        public Task<GetCodeResponse> GetListAsync(string codeTypeId, int? page);
    }
}

