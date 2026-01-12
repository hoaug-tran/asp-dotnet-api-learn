using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnLinQWeb.Migrations
{
    /// <inheritdoc />
    public partial class AddPhoneField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "phone",
                table: "users",
                type: "varchar(10)",
                maxLength: 10,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "phone",
                table: "users");
        }
    }
}
