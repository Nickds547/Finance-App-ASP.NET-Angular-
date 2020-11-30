using Microsoft.EntityFrameworkCore.Migrations;

namespace server.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserObjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    password = table.Column<string>(type: "nvarchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserObjects", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserObjects_Email",
                table: "UserObjects",
                column: "Email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserObjects");
        }
    }
}
