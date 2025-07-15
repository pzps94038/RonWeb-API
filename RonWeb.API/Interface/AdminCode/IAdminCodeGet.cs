using System;
using RonWeb.Database.Entities;

namespace RonWeb.API.Interface.AdminCode
{
    public interface IAdminCodeGet
    {
        public Task<VwCode> GetAsync(long id);
    }
}