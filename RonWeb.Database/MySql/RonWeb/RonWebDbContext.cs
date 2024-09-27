using Microsoft.EntityFrameworkCore;
using RonWeb.Core;
using RonWeb.Database.Enum;
using RonWeb.Database.MySql.RonWeb.Table;

namespace RonWeb.Database.MySql.RonWeb.DataBase
{
    public class RonWebDbContext : DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = Environment.GetEnvironmentVariable(MySqlDbEnum.RON_WEB_MYSQL_DB_CONSTR.Description())!;
            optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 33)),
            options => options.EnableRetryOnFailure(1));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 定義雙主鍵
            modelBuilder.Entity<RefreshTokenLog>()
                .HasKey(a => new { a.RefreshToken, a.UserId });
            modelBuilder.Entity<ArticleLabelMapping>()
                .HasKey(a => new { a.ArticleId, a.LabelId });
            modelBuilder.Entity<ProjectRole>()
                .HasKey(a => new { a.ProjectExperienceId, a.RoleId });
            modelBuilder.Entity<TechnologyTool>()
                .HasKey(a => new { a.ProjectExperienceId, a.TechnologyToolId });
            // 定義一對多關聯
            modelBuilder.Entity<Article>()
                .HasMany(a => a.ArticleReferences)
                .WithOne(r => r.Article)
                .HasForeignKey(r => r.ArticleId);
            modelBuilder.Entity<CodeType>()
                .HasMany(a => a.Codes)
                .WithOne(r => r.CodeType)
                .HasForeignKey(r => r.CodeTypeId);
            modelBuilder.Entity<ProjectExperience>()
                .HasMany(a => a.ProjectRole)
                .WithOne(r => r.ProjectExperience)
                .HasForeignKey(r => r.ProjectExperienceId);
            modelBuilder.Entity<ProjectExperience>()
                .HasMany(a => a.TechnologyTool)
                .WithOne(r => r.ProjectExperience)
                .HasForeignKey(r => r.ProjectExperienceId);
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
        public DbSet<ArticleReferences> ArticleReferences { get; set; }
        public DbSet<CodeType> CodeType { get; set; }
        public DbSet<Code> Code { get; set; }
        public DbSet<ProjectExperience> ProjectExperience { get; set; }
        public DbSet<ProjectRole> ProjectRole { get; set; }
        public DbSet<TechnologyTool> TechnologyTool { get; set; }

    }
}

