using System;
using RonWeb.API.Interface.Shared;
using RonWeb.API.Models.ArticleLabel;
using RonWeb.API.Models.Shared;

namespace RonWeb.API.Interface.ArticleLabel
{
    public interface IArticleLabelHelper : IGetListAsync<Label>, ICreateAsync<CreateArticleLabelRequest>, IUpdateAsync<Label>, IDeleteAsync<string>
    {
    }
}

