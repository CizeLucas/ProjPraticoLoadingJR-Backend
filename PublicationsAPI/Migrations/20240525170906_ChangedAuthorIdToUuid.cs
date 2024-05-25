using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PublicationsAPI.Migrations
{
    /// <inheritdoc />
    public partial class ChangedAuthorIdToUuid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuthorUuid",
                table: "Publications",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthorUuid",
                table: "Publications");
        }
    }
}
