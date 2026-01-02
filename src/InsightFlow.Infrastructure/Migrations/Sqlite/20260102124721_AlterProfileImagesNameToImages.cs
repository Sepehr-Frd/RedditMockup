#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace InsightFlow.Infrastructure.Migrations.Sqlite
{
    /// <inheritdoc />
    public partial class AlterProfileImagesNameToImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogPosts_ProfileImages_CoverImageId",
                table: "BlogPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_ProfileImages_ProfileImageId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProfileImages",
                table: "ProfileImages");

            migrationBuilder.RenameTable(
                name: "ProfileImages",
                newName: "Images");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Images",
                table: "Images",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPosts_Images_CoverImageId",
                table: "BlogPosts",
                column: "CoverImageId",
                principalTable: "Images",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Images_ProfileImageId",
                table: "Users",
                column: "ProfileImageId",
                principalTable: "Images",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogPosts_Images_CoverImageId",
                table: "BlogPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Images_ProfileImageId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Images",
                table: "Images");

            migrationBuilder.RenameTable(
                name: "Images",
                newName: "ProfileImages");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProfileImages",
                table: "ProfileImages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPosts_ProfileImages_CoverImageId",
                table: "BlogPosts",
                column: "CoverImageId",
                principalTable: "ProfileImages",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_ProfileImages_ProfileImageId",
                table: "Users",
                column: "ProfileImageId",
                principalTable: "ProfileImages",
                principalColumn: "Id");
        }
    }
}