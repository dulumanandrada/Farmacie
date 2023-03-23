using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Farmacie.Data.Migrations
{
    public partial class AddStatusToCommand : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Commands",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Commands");
        }
    }
}
