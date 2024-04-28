using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.UpCareData.Migrations
{
    public partial class ModifyDoctorAndPatientInPrescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DoctorGivePrescriptions");

            migrationBuilder.AddColumn<string>(
                name: "FK_DoctorId",
                table: "Prescriptions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FK_PatientId",
                table: "Prescriptions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FK_DoctorId",
                table: "Prescriptions");

            migrationBuilder.DropColumn(
                name: "FK_PatientId",
                table: "Prescriptions");

            migrationBuilder.CreateTable(
                name: "DoctorGivePrescriptions",
                columns: table => new
                {
                    FK_PatientId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FK_PrescriptionId = table.Column<int>(type: "int", nullable: false),
                    FK_DoctorId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorGivePrescriptions", x => new { x.FK_PatientId, x.FK_PrescriptionId, x.FK_DoctorId });
                });
        }
    }
}
