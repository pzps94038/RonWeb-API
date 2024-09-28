using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RonWeb.Database.Entities;

public partial class RonWebDbContext : DbContext
{
    public RonWebDbContext(DbContextOptions<RonWebDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Article> Article { get; set; }

    public virtual DbSet<ArticleCategory> ArticleCategory { get; set; }

    public virtual DbSet<ArticleImage> ArticleImage { get; set; }

    public virtual DbSet<ArticleLabel> ArticleLabel { get; set; }

    public virtual DbSet<ArticleLabelMapping> ArticleLabelMapping { get; set; }

    public virtual DbSet<ArticleReferences> ArticleReferences { get; set; }

    public virtual DbSet<Code> Code { get; set; }

    public virtual DbSet<CodeType> CodeType { get; set; }

    public virtual DbSet<ExceptionLog> ExceptionLog { get; set; }

    public virtual DbSet<ProjectExperience> ProjectExperience { get; set; }

    public virtual DbSet<ProjectExperienceImage> ProjectExperienceImage { get; set; }

    public virtual DbSet<ProjectRole> ProjectRole { get; set; }

    public virtual DbSet<RefreshTokenLog> RefreshTokenLog { get; set; }

    public virtual DbSet<TechnologyTool> TechnologyTool { get; set; }

    public virtual DbSet<UserMain> UserMain { get; set; }

    public virtual DbSet<VwArticle> VwArticle { get; set; }

    public virtual DbSet<VwCode> VwCode { get; set; }

    public virtual DbSet<VwRefreshTokenLog> VwRefreshTokenLog { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Article>(entity =>
        {
            entity.HasKey(e => e.ArticleId).HasName("PRIMARY");

            entity.ToTable(tb => tb.HasComment("文章"));

            entity.HasIndex(e => e.CategoryId, "IX_Article_CategoryId");

            entity.Property(e => e.ArticleId).HasComment("文章Id");
            entity.Property(e => e.ArticleTitle).HasComment("文章標題");
            entity.Property(e => e.CategoryId).HasComment("類別Id");
            entity.Property(e => e.Content).HasComment("文章內容");
            entity.Property(e => e.CreateBy).HasComment("創建人員");
            entity.Property(e => e.CreateDate)
                .HasMaxLength(6)
                .HasComment("創建日期");
            entity.Property(e => e.Flag)
                .HasMaxLength(1)
                .HasDefaultValueSql("''")
                .HasComment("Y/N");
            entity.Property(e => e.PreviewContent).HasComment("預覽文章內容");
            entity.Property(e => e.UpdateBy).HasComment("修改人員");
            entity.Property(e => e.UpdateDate)
                .HasMaxLength(6)
                .HasComment("修改日期");
            entity.Property(e => e.ViewCount).HasComment("瀏覽數");
        });

        modelBuilder.Entity<ArticleCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PRIMARY");

            entity.ToTable(tb => tb.HasComment("文章類別"));

            entity.Property(e => e.CategoryId).HasComment("類別Id");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(20)
                .HasDefaultValueSql("''")
                .HasComment("類別名稱");
            entity.Property(e => e.CreateBy).HasComment("創建人員");
            entity.Property(e => e.CreateDate)
                .HasMaxLength(6)
                .HasComment("創建日期");
            entity.Property(e => e.UpdateBy).HasComment("修改人員");
            entity.Property(e => e.UpdateDate)
                .HasMaxLength(6)
                .HasComment("修改日期");
        });

        modelBuilder.Entity<ArticleImage>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("PRIMARY");

            entity.ToTable(tb => tb.HasComment("文章圖片"));

            entity.HasIndex(e => e.ArticleId, "IX_ArticleImage_ArticleId");

            entity.Property(e => e.ImageId).HasComment("圖片Id");
            entity.Property(e => e.ArticleId).HasComment("文章Id");
            entity.Property(e => e.CreateBy).HasComment("創建人員");
            entity.Property(e => e.CreateDate)
                .HasMaxLength(6)
                .HasComment("創建日期");
            entity.Property(e => e.FileName)
                .HasMaxLength(50)
                .HasComment("檔案名稱");
            entity.Property(e => e.Path).HasComment("路徑");
            entity.Property(e => e.UpdateBy).HasComment("修改人員");
            entity.Property(e => e.UpdateDate)
                .HasMaxLength(6)
                .HasComment("修改日期");
            entity.Property(e => e.Url).HasComment("對外網址");
        });

        modelBuilder.Entity<ArticleLabel>(entity =>
        {
            entity.HasKey(e => e.LabelId).HasName("PRIMARY");

            entity.ToTable(tb => tb.HasComment("文章標籤"));

            entity.Property(e => e.LabelId).HasComment("標籤Id");
            entity.Property(e => e.CreateBy).HasComment("創建人員");
            entity.Property(e => e.CreateDate)
                .HasMaxLength(6)
                .HasComment("創建日期");
            entity.Property(e => e.LabelName)
                .HasMaxLength(20)
                .HasDefaultValueSql("''")
                .HasComment("標籤名稱");
            entity.Property(e => e.UpdateBy).HasComment("更新人員");
            entity.Property(e => e.UpdateDate)
                .HasMaxLength(6)
                .HasComment("更新日期");
        });

        modelBuilder.Entity<ArticleLabelMapping>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable(tb => tb.HasComment("文章標籤對應表"));

            entity.HasIndex(e => e.LabelId, "IX_ArticleLabelMapping_LabelId");

            entity.Property(e => e.ArticleId).HasComment("文章Id");
            entity.Property(e => e.CreateBy).HasComment("創建人員");
            entity.Property(e => e.CreateDate)
                .HasMaxLength(6)
                .HasComment("創建日期");
            entity.Property(e => e.LabelId).HasComment("標籤Id");
            entity.Property(e => e.UpdateBy).HasComment("更新人員");
            entity.Property(e => e.UpdateDate)
                .HasMaxLength(6)
                .HasComment("更新日期");
        });

        modelBuilder.Entity<ArticleReferences>(entity =>
        {
            entity.HasKey(e => e.ArticleReferencesId).HasName("PRIMARY");

            entity.ToTable(tb => tb.HasComment("參考文章表"));

            entity.HasIndex(e => e.ArticleId, "IX_ArticleReferences_ArticleId");

            entity.Property(e => e.ArticleReferencesId).HasComment("參考文章Id");
            entity.Property(e => e.ArticleId).HasComment("文章Id");
            entity.Property(e => e.CreateBy).HasComment("創建人員");
            entity.Property(e => e.CreateDate)
                .HasMaxLength(6)
                .HasComment("創建日期");
            entity.Property(e => e.Link).HasComment("參考連結");
            entity.Property(e => e.UpdateBy).HasComment("更新人員");
            entity.Property(e => e.UpdateDate)
                .HasMaxLength(6)
                .HasComment("更新日期");
        });

        modelBuilder.Entity<Code>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable(tb => tb.HasComment("代碼表"));

            entity.Property(e => e.Id).HasComment("主鍵");
            entity.Property(e => e.CodeId)
                .HasMaxLength(255)
                .HasComment("代碼Id");
            entity.Property(e => e.CodeName).HasComment("代碼名稱");
            entity.Property(e => e.CodeTypeId)
                .HasMaxLength(255)
                .HasComment("代碼類型Id");
            entity.Property(e => e.CreateBy).HasComment("創建人員");
            entity.Property(e => e.CreateDate)
                .HasMaxLength(6)
                .HasComment("創建日期");
            entity.Property(e => e.UpdateBy).HasComment("修改人員");
            entity.Property(e => e.UpdateDate)
                .HasMaxLength(6)
                .HasComment("修改日期");
        });

        modelBuilder.Entity<CodeType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable(tb => tb.HasComment("代碼類型表"));

            entity.Property(e => e.Id).HasComment("主鍵");
            entity.Property(e => e.CodeTypeId)
                .HasMaxLength(255)
                .HasComment("類型Id");
            entity.Property(e => e.CodeTypeName).HasComment("類型名稱");
            entity.Property(e => e.CreateBy).HasComment("創建人員");
            entity.Property(e => e.CreateDate)
                .HasMaxLength(6)
                .HasComment("創建日期");
            entity.Property(e => e.UpdateBy).HasComment("更新人員");
            entity.Property(e => e.UpdateDate)
                .HasMaxLength(6)
                .HasComment("更新日期");
        });

        modelBuilder.Entity<ExceptionLog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PRIMARY");

            entity.ToTable(tb => tb.HasComment("錯誤Log"));

            entity.Property(e => e.LogId).HasComment("主鍵Id");
            entity.Property(e => e.CreateDate)
                .HasMaxLength(6)
                .HasComment("創建日期");
            entity.Property(e => e.Level).HasComment("日誌等級");
            entity.Property(e => e.Message).HasComment("錯誤訊息");
            entity.Property(e => e.StackTrace).HasComment("Stack");
        });

        modelBuilder.Entity<ProjectExperience>(entity =>
        {
            entity.HasKey(e => e.ProjectExperienceId).HasName("PRIMARY");

            entity.ToTable(tb => tb.HasComment("專案經歷"));

            entity.Property(e => e.ProjectExperienceId).HasComment("專案經歷Id");
            entity.Property(e => e.Contributions).HasComment("專案貢獻");
            entity.Property(e => e.CreateBy).HasComment("創建人員");
            entity.Property(e => e.CreateDate)
                .HasMaxLength(6)
                .HasComment("創建日期");
            entity.Property(e => e.Description).HasComment("專案描述");
            entity.Property(e => e.Name).HasComment("專案名稱");
            entity.Property(e => e.UpdateBy).HasComment("修改人員");
            entity.Property(e => e.UpdateDate)
                .HasMaxLength(6)
                .HasComment("修改日期");
        });

        modelBuilder.Entity<ProjectExperienceImage>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("PRIMARY");

            entity.ToTable(tb => tb.HasComment("專案經歷圖片"));

            entity.Property(e => e.ImageId).HasComment("圖片Id");
            entity.Property(e => e.CreateBy).HasComment("創建人員");
            entity.Property(e => e.CreateDate)
                .HasMaxLength(6)
                .HasComment("創建日期");
            entity.Property(e => e.FileName)
                .HasMaxLength(50)
                .HasComment("檔案名稱");
            entity.Property(e => e.Path).HasComment("路徑");
            entity.Property(e => e.ProjectExperienceId).HasComment("專案經歷Id");
            entity.Property(e => e.UpdateBy).HasComment("修改人員");
            entity.Property(e => e.UpdateDate)
                .HasMaxLength(6)
                .HasComment("修改日期");
            entity.Property(e => e.Url).HasComment("對外網址");
        });

        modelBuilder.Entity<ProjectRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable(tb => tb.HasComment("專案角色"));

            entity.Property(e => e.Id).HasComment("主鍵");
            entity.Property(e => e.CreateBy).HasComment("創建人員");
            entity.Property(e => e.CreateDate)
                .HasMaxLength(6)
                .HasComment("創建日期");
            entity.Property(e => e.ProjectExperienceId).HasComment("專案經歷Id");
            entity.Property(e => e.RoleId)
                .HasMaxLength(255)
                .HasComment("角色Id");
            entity.Property(e => e.UpdateBy).HasComment("修改人員");
            entity.Property(e => e.UpdateDate)
                .HasMaxLength(6)
                .HasComment("修改日期");
        });

        modelBuilder.Entity<RefreshTokenLog>(entity =>
        {
            entity.HasKey(e => new { e.RefreshToken, e.UserId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable(tb => tb.HasComment("Token紀錄表"));

            entity.HasIndex(e => e.UserId, "IX_RefreshTokenLog_UserId");

            entity.Property(e => e.RefreshToken).HasComment("刷新Token");
            entity.Property(e => e.UserId).HasComment("Token擁有人");
            entity.Property(e => e.CreateDate)
                .HasMaxLength(6)
                .HasComment("創建日期");
            entity.Property(e => e.ExpirationDate)
                .HasMaxLength(6)
                .HasComment("過期日期");

            entity.HasOne(d => d.User).WithMany(p => p.RefreshTokenLog).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<TechnologyTool>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable(tb => tb.HasComment("專案技術"));

            entity.Property(e => e.Id).HasComment("主鍵");
            entity.Property(e => e.CreateBy).HasComment("創建人員");
            entity.Property(e => e.CreateDate)
                .HasMaxLength(6)
                .HasComment("創建日期");
            entity.Property(e => e.ProjectExperienceId).HasComment("專案經驗Id");
            entity.Property(e => e.TechnologyToolId)
                .HasMaxLength(255)
                .HasComment("使用技術Id");
            entity.Property(e => e.UpdateBy).HasComment("修改人員");
            entity.Property(e => e.UpdateDate)
                .HasMaxLength(6)
                .HasComment("修改日期");
        });

        modelBuilder.Entity<UserMain>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable(tb => tb.HasComment("使用者"));

            entity.Property(e => e.UserId).HasComment("使用者Id");
            entity.Property(e => e.Account)
                .HasMaxLength(50)
                .HasComment("帳號");
            entity.Property(e => e.CreateDate)
                .HasMaxLength(6)
                .HasComment("創建日期");
            entity.Property(e => e.Email)
                .HasMaxLength(250)
                .HasComment("Email");
            entity.Property(e => e.Password)
                .HasMaxLength(250)
                .HasComment("密碼");
            entity.Property(e => e.UpdateDate)
                .HasMaxLength(6)
                .HasComment("修改日期");
            entity.Property(e => e.UserName)
                .HasMaxLength(20)
                .HasComment("使用者名稱");
        });

        modelBuilder.Entity<VwArticle>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_Article");

            entity.Property(e => e.ArticleCreateDate)
                .HasMaxLength(6)
                .HasComment("創建日期");
            entity.Property(e => e.ArticleId).HasComment("文章Id");
            entity.Property(e => e.ArticleTitle).HasComment("文章標題");
            entity.Property(e => e.ArticleUpdateDate)
                .HasMaxLength(6)
                .HasComment("修改日期");
            entity.Property(e => e.CategoryId).HasComment("類別Id");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(20)
                .HasDefaultValueSql("''")
                .HasComment("類別名稱");
            entity.Property(e => e.Content).HasComment("文章內容");
            entity.Property(e => e.Flag)
                .HasMaxLength(1)
                .HasDefaultValueSql("''")
                .HasComment("Y/N");
            entity.Property(e => e.LabelCreateDate)
                .HasMaxLength(6)
                .HasComment("創建日期");
            entity.Property(e => e.LabelId)
                .HasDefaultValueSql("'0'")
                .HasComment("標籤Id");
            entity.Property(e => e.LabelName)
                .HasMaxLength(20)
                .HasDefaultValueSql("''")
                .HasComment("標籤名稱");
            entity.Property(e => e.LabelUpdateDate)
                .HasMaxLength(6)
                .HasComment("更新日期");
            entity.Property(e => e.Link).HasComment("參考連結");
            entity.Property(e => e.PreviewContent).HasComment("預覽文章內容");
            entity.Property(e => e.ViewCount).HasComment("瀏覽數");
        });

        modelBuilder.Entity<VwCode>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_Code");

            entity.Property(e => e.CodeId)
                .HasMaxLength(255)
                .HasComment("代碼Id");
            entity.Property(e => e.CodeName).HasComment("代碼名稱");
            entity.Property(e => e.CodeTypeId)
                .HasMaxLength(255)
                .HasComment("類型Id");
            entity.Property(e => e.CodeTypeName).HasComment("類型名稱");
            entity.Property(e => e.CreateBy).HasComment("創建人員");
            entity.Property(e => e.CreateDate)
                .HasMaxLength(6)
                .HasComment("創建日期");
            entity.Property(e => e.Id).HasComment("主鍵");
            entity.Property(e => e.UpdateBy).HasComment("修改人員");
            entity.Property(e => e.UpdateDate)
                .HasMaxLength(6)
                .HasComment("修改日期");
        });

        modelBuilder.Entity<VwRefreshTokenLog>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_RefreshTokenLog");

            entity.Property(e => e.Account)
                .HasMaxLength(50)
                .HasComment("帳號");
            entity.Property(e => e.Email)
                .HasMaxLength(250)
                .HasComment("Email");
            entity.Property(e => e.ExpirationDate)
                .HasMaxLength(6)
                .HasComment("過期日期");
            entity.Property(e => e.RefreshToken)
                .HasMaxLength(255)
                .HasComment("刷新Token");
            entity.Property(e => e.UserId).HasComment("Token擁有人");
            entity.Property(e => e.UserName)
                .HasMaxLength(20)
                .HasComment("使用者名稱");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
