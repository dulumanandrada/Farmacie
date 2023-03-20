using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Farmacie.Data.Migrations
{
    public partial class ManyToManyChange2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MedicamentCommands",
                table: "MedicamentCommands");

            migrationBuilder.DropIndex(
                name: "IX_MedicamentCommands_MedicamentId",
                table: "MedicamentCommands");

            migrationBuilder.DropColumn(
                name: "MedicamentCommandId",
                table: "MedicamentCommands");

            migrationBuilder.AddColumn<int>(
                name: "QuantityWanted",
                table: "MedicamentCommands",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MedicamentCommands",
                table: "MedicamentCommands",
                columns: new[] { "MedicamentId", "CommandId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MedicamentCommands",
                table: "MedicamentCommands");

            migrationBuilder.DropColumn(
                name: "QuantityWanted",
                table: "MedicamentCommands");

            migrationBuilder.AddColumn<int>(
                name: "MedicamentCommandId",
                table: "MedicamentCommands",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MedicamentCommands",
                table: "MedicamentCommands",
                columns: new[] { "MedicamentCommandId", "MedicamentId", "CommandId" });

            migrationBuilder.CreateIndex(
                name: "IX_MedicamentCommands_MedicamentId",
                table: "MedicamentCommands",
                column: "MedicamentId");
        }
    }
}
