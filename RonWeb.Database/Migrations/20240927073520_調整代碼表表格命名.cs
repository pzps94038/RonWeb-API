using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RonWeb.Database.Migrations
{
    /// <inheritdoc />
    public partial class 調整代碼表表格命名 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Codes_CodeTypes_CodeTypeId",
                table: "Codes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CodeTypes",
                table: "CodeTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Codes",
                table: "Codes");

            migrationBuilder.RenameTable(
                name: "CodeTypes",
                newName: "CodeType");

            migrationBuilder.RenameTable(
                name: "Codes",
                newName: "Code");

            migrationBuilder.RenameIndex(
                name: "IX_Codes_CodeTypeId",
                table: "Code",
                newName: "IX_Code_CodeTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CodeType",
                table: "CodeType",
                column: "CodeTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Code",
                table: "Code",
                column: "CodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Code_CodeType_CodeTypeId",
                table: "Code",
                column: "CodeTypeId",
                principalTable: "CodeType",
                principalColumn: "CodeTypeId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Code_CodeType_CodeTypeId",
                table: "Code");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CodeType",
                table: "CodeType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Code",
                table: "Code");

            migrationBuilder.RenameTable(
                name: "CodeType",
                newName: "CodeTypes");

            migrationBuilder.RenameTable(
                name: "Code",
                newName: "Codes");

            migrationBuilder.RenameIndex(
                name: "IX_Code_CodeTypeId",
                table: "Codes",
                newName: "IX_Codes_CodeTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CodeTypes",
                table: "CodeTypes",
                column: "CodeTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Codes",
                table: "Codes",
                column: "CodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Codes_CodeTypes_CodeTypeId",
                table: "Codes",
                column: "CodeTypeId",
                principalTable: "CodeTypes",
                principalColumn: "CodeTypeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
