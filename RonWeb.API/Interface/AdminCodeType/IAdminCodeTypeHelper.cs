using System;
using RonWeb.API.Interface.Shared;
using RonWeb.API.Models.Article;
using RonWeb.API.Models.ArticleCategory;
using RonWeb.API.Models.ArticleLabel;
using RonWeb.API.Models.CodeType;
using RonWeb.Database.Entities;

namespace RonWeb.API.Interface.AdminArticleHelper
{
    public interface IAdminCodeTypeHelper :
        IGetAsync<string, CodeType>,
        ICreateAsync<CreateCodeTypeRequest>,
        IUpdateAsync<string, UpdateCodeTypeRequest>,
        IDeleteAsync<string>
    {
        public Task<GetCodeTypeResponse> GetListAsync(int? page);
    }
}

