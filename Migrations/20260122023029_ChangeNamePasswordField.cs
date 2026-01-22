using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnLinQWeb.Migrations
{
    /// <inheritdoc />
    public partial class ChangeNamePasswordField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "password",
                table: "users",
                newName: "password_hash");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "password_hash",
                table: "users",
                newName: "password");
        }

    }
}
