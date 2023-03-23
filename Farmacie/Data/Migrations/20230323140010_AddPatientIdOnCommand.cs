using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Farmacie.Data.Migrations
{
    public partial class AddPatientIdOnCommand : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PatientId",
                table: "Commands",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PatientId1",
                table: "Commands",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Commands_PatientId1",
                table: "Commands",
                column: "PatientId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Commands_Patients_PatientId1",
                table: "Commands",
                column: "PatientId1",
                principalTable: "Patients",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commands_Patients_PatientId1",
                table: "Commands");

            migrationBuilder.DropIndex(
                name: "IX_Commands_PatientId1",
                table: "Commands");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "Commands");

            migrationBuilder.DropColumn(
                name: "PatientId1",
                table: "Commands");
        }
    }
}
