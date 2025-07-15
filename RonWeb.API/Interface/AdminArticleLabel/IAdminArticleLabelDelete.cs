using System;

namespace RonWeb.API.Interface.AdminArticleLabel
{
    public interface IAdminArticleLabelDelete
    {
        public Task DeleteAsync(long data);
    }
}