using Microsoft.EntityFrameworkCore;
using RonWeb.API.Interface.AdminArticleHelper;
using RonWeb.Database.Entities;
namespace RonWeb.API.Helper.AdminArticleLabel
{
    public class AdminCodeHelper : IAdminCodeHelper
    {
        private readonly RonWebDbContext _db;

        public AdminCodeHelper(RonWebDbContext dbContext)
        {
            _db = dbContext;
        }
    }
}

