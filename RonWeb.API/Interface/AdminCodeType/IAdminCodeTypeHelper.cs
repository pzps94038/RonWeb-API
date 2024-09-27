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
        IGetAsync<long, CodeType>,
        ICreateAsync<CreateCodeTypeRequest>,
        IUpdateAsync<long, UpdateCodeTypeRequest>,
        IDeleteAsync<long>
    {
        public Task<GetCodeTypeResponse> GetListAsync(int? page);
    }
}

