using System;
using RonWeb.Database.Entities;

namespace RonWeb.API.Interface.AdminCodeType
{
    public interface IAdminCodeTypeGet
    {
        public Task<CodeType> GetAsync(long id);
    }
}