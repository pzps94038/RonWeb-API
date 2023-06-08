using Microsoft.EntityFrameworkCore;
using RonWeb.Core;
using RonWeb.Database.MySql.Enum;
using RonWeb.Database.MySql.RonWeb.Table;

namespace RonWeb.Database.MySql.RonWeb.DataBase
{
    public class RonWebDbContext: DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = Environment.GetEnvironmentVariable(MySqlDbEnum.RON_WEB_MYSQL_DB_CONSTR.Description())!;
            optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 33)),
            options => options.EnableRetryOnFailure(1));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RefreshTokenLog>()
                .HasKey(a => new { a.RefreshToken, a.UserId });
            modelBuilder.Entity<ArticleLabelMapping>()
               .HasKey(a => new { a.ArticleId, a.LabelId });
        }

        public DbSet<Article> Article { get; set; }
        public DbSet<ArticleCategory> ArticleCategory { get; set; }
        public DbSet<ArticleImage> ArticleImage { get; set; }
        public DbSet<ArticleLabel> ArticleLabel { get; set; }
        public DbSet<ArticleLabelMapping> ArticleLabelMapping { get; set; }
        public DbSet<ArticlePrevImage> ArticlePrevImage { get; set; }
        public DbSet<UserMain> UserMain { get; set; }
        public DbSet<RefreshTokenLog> RefreshTokenLog { get; set; }
        public DbSet<ExceptionLog> ExceptionLog { get; set; }
        
    }
}

