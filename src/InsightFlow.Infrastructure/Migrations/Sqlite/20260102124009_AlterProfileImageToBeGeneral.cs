#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace InsightFlow.Infrastructure.Migrations.Sqlite
{
    /// <inheritdoc />
    public partial class AlterProfileImageToBeGeneral : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProfileImages_Users_UserId",
                table: "ProfileImages");

            migrationBuilder.DropIndex(
                name: "IX_ProfileImages_UserId",
                table: "ProfileImages");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ProfileImages",
                newName: "Type");

            migrationBuilder.AddColumn<long>(
                name: "ProfileImageId",
                table: "Users",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "OwnerId",
                table: "ProfileImages",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "CoverImageId",
                table: "BlogPosts",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_ProfileImageId",
                table: "Users",
                column: "ProfileImageId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogPosts_CoverImageId",
                table: "BlogPosts",
                column: "CoverImageId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogPosts_ProfileImages_CoverImageId",
                table: "BlogPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_ProfileImages_ProfileImageId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_ProfileImageId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_BlogPosts_CoverImageId",
                table: "BlogPosts");

            migrationBuilder.DropColumn(
                name: "ProfileImageId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "ProfileImages");

            migrationBuilder.DropColumn(
                name: "CoverImageId",
                table: "BlogPosts");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "ProfileImages",
                newName: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileImages_UserId",
                table: "ProfileImages",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileImages_Users_UserId",
                table: "ProfileImages",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}