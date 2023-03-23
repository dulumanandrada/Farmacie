using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Farmacie.Data.Migrations
{
    public partial class AddPatientIdOnCommand4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PatientName",
                table: "Commands",
                newName: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Commands",
                newName: "PatientName");
        }
    }
}
