using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.UpCareData.Migrations
{
    public partial class AddAppointmentConsultationAndDoingOperationsTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DoctorDoOperations",
                columns: table => new
                {
                    FK_OperationId = table.Column<int>(type: "int", nullable: false),
                    FK_DoctorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FK_PatientId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorDoOperations", x => new { x.FK_PatientId, x.FK_DoctorId, x.FK_OperationId, x.Date });
                });

            migrationBuilder.CreateTable(
                name: "PatientAppointments",
                columns: table => new
                {
                    FK_PatientId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FK_DoctorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientAppointments", x => new { x.FK_PatientId, x.FK_DoctorId, x.DateTime });
                });

            migrationBuilder.CreateTable(
                name: "PatientConsultations",
                columns: table => new
                {
                    FK_DoctorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FK_PatientId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientConsultations", x => new { x.FK_PatientId, x.FK_DoctorId, x.DateTime });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DoctorDoOperations");

            migrationBuilder.DropTable(
                name: "PatientAppointments");

            migrationBuilder.DropTable(
                name: "PatientConsultations");
        }
    }
}
