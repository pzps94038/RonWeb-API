using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RonWeb.Database.Migrations
{
    /// <inheritdoc />
    public partial class initiaf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ArticleCategory",
                columns: table => new
                {
                    CategoryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreateBy = table.Column<long>(type: "bigint", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UpdateBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleCategory", x => x.CategoryId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ArticleLabel",
                columns: table => new
                {
                    LabelId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreateBy = table.Column<long>(type: "bigint", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UpdateBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleLabel", x => x.LabelId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ExceptionLog",
                columns: table => new
                {
                    LogId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Message = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StackTrace = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Level = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExceptionLog", x => x.LogId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserMain",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Account = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Password = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserName = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMain", x => x.UserId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Article",
                columns: table => new
                {
                    ArticleId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ArticleTitle = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PreviewContent = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Content = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CategoryId = table.Column<long>(type: "bigint", nullable: false),
                    ViewCount = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreateBy = table.Column<long>(type: "bigint", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UpdateBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Article", x => x.ArticleId);
                    table.ForeignKey(
                        name: "FK_Article_ArticleCategory_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "ArticleCategory",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RefreshTokenLog",
                columns: table => new
                {
                    RefreshToken = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokenLog", x => new { x.RefreshToken, x.UserId });
                    table.ForeignKey(
                        name: "FK_RefreshTokenLog_UserMain_UserId",
                        column: x => x.UserId,
                        principalTable: "UserMain",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ArticleImage",
                columns: table => new
                {
                    ImageId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ArticleId = table.Column<long>(type: "bigint", nullable: false),
                    FileName = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Path = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Url = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreateBy = table.Column<long>(type: "bigint", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UpdateBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleImage", x => x.ImageId);
                    table.ForeignKey(
                        name: "FK_ArticleImage_Article_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Article",
                        principalColumn: "ArticleId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ArticleLabelMapping",
                columns: table => new
                {
                    LabelId = table.Column<long>(type: "bigint", nullable: false),
                    ArticleId = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreateBy = table.Column<long>(type: "bigint", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UpdateBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleLabelMapping", x => new { x.ArticleId, x.LabelId });
                    table.ForeignKey(
                        name: "FK_ArticleLabelMapping_ArticleLabel_LabelId",
                        column: x => x.LabelId,
                        principalTable: "ArticleLabel",
                        principalColumn: "LabelId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArticleLabelMapping_Article_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Article",
                        principalColumn: "ArticleId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ArticlePrevImage",
                columns: table => new
                {
                    ImageId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ArticleId = table.Column<long>(type: "bigint", nullable: false),
                    FileName = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Path = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Url = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreateBy = table.Column<long>(type: "bigint", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UpdateBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticlePrevImage", x => x.ImageId);
                    table.ForeignKey(
                        name: "FK_ArticlePrevImage_Article_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Article",
                        principalColumn: "ArticleId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Article_CategoryId",
                table: "Article",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleImage_ArticleId",
                table: "ArticleImage",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleLabelMapping_LabelId",
                table: "ArticleLabelMapping",
                column: "LabelId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticlePrevImage_ArticleId",
                table: "ArticlePrevImage",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokenLog_UserId",
                table: "RefreshTokenLog",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticleImage");

            migrationBuilder.DropTable(
                name: "ArticleLabelMapping");

            migrationBuilder.DropTable(
                name: "ArticlePrevImage");

            migrationBuilder.DropTable(
                name: "ExceptionLog");

            migrationBuilder.DropTable(
                name: "RefreshTokenLog");

            migrationBuilder.DropTable(
                name: "ArticleLabel");

            migrationBuilder.DropTable(
                name: "Article");

            migrationBuilder.DropTable(
                name: "UserMain");

            migrationBuilder.DropTable(
                name: "ArticleCategory");
        }
    }
}
