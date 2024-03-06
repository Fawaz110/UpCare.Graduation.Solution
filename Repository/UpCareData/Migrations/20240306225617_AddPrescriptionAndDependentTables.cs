using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.UpCareData.Migrations
{
    public partial class AddPrescriptionAndDependentTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CheckupInPrescriptions",
                columns: table => new
                {
                    FK_CheckupId = table.Column<int>(type: "int", nullable: false),
                    FK_PrescriptionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckupInPrescriptions", x => new { x.FK_CheckupId, x.FK_PrescriptionId });
                });

            migrationBuilder.CreateTable(
                name: "DoctorGivePrescriptions",
                columns: table => new
                {
                    FK_PatientId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FK_DoctorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FK_PrescriptionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorGivePrescriptions", x => new { x.FK_PatientId, x.FK_PrescriptionId, x.FK_DoctorId });
                });

            migrationBuilder.CreateTable(
                name: "MedicineInPrescriptions",
                columns: table => new
                {
                    FK_MedicineId = table.Column<int>(type: "int", nullable: false),
                    FK_PrescriptionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicineInPrescriptions", x => new { x.FK_MedicineId, x.FK_PrescriptionId });
                });

            migrationBuilder.CreateTable(
                name: "Prescriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Diagnosis = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Advice = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prescriptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RadiologyInPrescriptions",
                columns: table => new
                {
                    FK_RadiologyId = table.Column<int>(type: "int", nullable: false),
                    FK_PrescriptionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RadiologyInPrescriptions", x => new { x.FK_PrescriptionId, x.FK_RadiologyId });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CheckupInPrescriptions");

            migrationBuilder.DropTable(
                name: "DoctorGivePrescriptions");

            migrationBuilder.DropTable(
                name: "MedicineInPrescriptions");

            migrationBuilder.DropTable(
                name: "Prescriptions");

            migrationBuilder.DropTable(
                name: "RadiologyInPrescriptions");
        }
    }
}
