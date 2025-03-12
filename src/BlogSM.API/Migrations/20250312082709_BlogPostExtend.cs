using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogSM.API.Migrations
{
    /// <inheritdoc />
    public partial class BlogPostExtend : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "URLTitle",
                table: "BlogPosts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "URLTitle",
                table: "BlogPosts");
        }
    }
}
