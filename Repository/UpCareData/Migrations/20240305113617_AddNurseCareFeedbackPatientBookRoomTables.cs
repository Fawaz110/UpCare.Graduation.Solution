using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.UpCareData.Migrations
{
    public partial class AddNurseCareFeedbackPatientBookRoomTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Feedbacks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FK_PatientId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedbacks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NurseCares",
                columns: table => new
                {
                    FK_NurseId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FK_PatientId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FK_RoomId = table.Column<int>(type: "int", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Suger = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BloodPresure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NurseCares", x => new { x.FK_NurseId, x.FK_RoomId, x.FK_PatientId, x.DateTime });
                });

            migrationBuilder.CreateTable(
                name: "PatientBookRooms",
                columns: table => new
                {
                    FK_RoomId = table.Column<int>(type: "int", nullable: false),
                    FK_PatientId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FK_DoctorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientBookRooms", x => new { x.FK_PatientId, x.FK_DoctorId, x.FK_RoomId });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Feedbacks");

            migrationBuilder.DropTable(
                name: "NurseCares");

            migrationBuilder.DropTable(
                name: "PatientBookRooms");
        }
    }
}
