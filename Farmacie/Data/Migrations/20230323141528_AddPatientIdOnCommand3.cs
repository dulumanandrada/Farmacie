using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Farmacie.Data.Migrations
{
    public partial class AddPatientIdOnCommand3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commands_Patients_PatientId1",
                table: "Commands");

            migrationBuilder.DropIndex(
                name: "IX_Commands_PatientId1",
                table: "Commands");

            migrationBuilder.DropColumn(
                name: "PatientId1",
                table: "Commands");

            migrationBuilder.AlterColumn<string>(
                name: "PatientId",
                table: "Commands",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commands_Patients_PatientId",
                table: "Commands");

            migrationBuilder.DropIndex(
                name: "IX_Commands_PatientId",
                table: "Commands");

            migrationBuilder.AlterColumn<int>(
                name: "PatientId",
                table: "Commands",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

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
    }
}
