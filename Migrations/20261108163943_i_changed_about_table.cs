using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MYAcademy_Portfolio.Migrations
{
    /// <inheritdoc />
    public partial class i_changed_about_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Expert",
                table: "Abouts");

            migrationBuilder.RenameColumn(
                name: "OpenToWork",
                table: "Abouts",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "NameSurname",
                table: "Abouts",
                newName: "Phone");

            migrationBuilder.RenameColumn(
                name: "JobTitle",
                table: "Abouts",
                newName: "ImageUrl");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Abouts",
                newName: "OpenToWork");

            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "Abouts",
                newName: "NameSurname");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Abouts",
                newName: "JobTitle");

            migrationBuilder.AddColumn<string>(
                name: "Expert",
                table: "Abouts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
