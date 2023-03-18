using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Farmacie.Data.Migrations
{
    public partial class ModifyCommandMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commands_Patients_PatientId",
                table: "Commands");

            migrationBuilder.DropIndex(
                name: "IX_Commands_PatientId",
                table: "Commands");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "Commands");

            migrationBuilder.AddColumn<string>(
                name: "PatientName",
                table: "Commands",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PatientName",
                table: "Commands");

            migrationBuilder.AddColumn<int>(
                name: "PatientId",
                table: "Commands",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Commands_PatientId",
                table: "Commands",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Commands_Patients_PatientId",
                table: "Commands",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
