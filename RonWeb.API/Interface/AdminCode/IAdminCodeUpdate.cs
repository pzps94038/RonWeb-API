using System;
using RonWeb.API.Models.Code;

namespace RonWeb.API.Interface.AdminCode
{
    public interface IAdminCodeUpdate
    {
        public Task UpdateAsync(long id, UpdateCodeRequest data);
    }
}