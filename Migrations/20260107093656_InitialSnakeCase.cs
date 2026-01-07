using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnLinQWeb.Migrations
{
    /// <inheritdoc />
    public partial class InitialSnakeCase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "books",
            //    columns: table => new
            //    {
            //        id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        title = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        author = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("pk_books", x => x.id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "users",
            //    columns: table => new
            //    {
            //        id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        name = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        username = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        password = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        role = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        email = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("pk_users", x => x.id);
            //    });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "books");

            //migrationBuilder.DropTable(
            //    name: "users");
        }
    }
}
