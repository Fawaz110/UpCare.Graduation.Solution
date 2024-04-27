using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.UpCareData.Migrations
{
    public partial class updatesInDoctorDoOperationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DoctorDoOperations",
                table: "DoctorDoOperations");

            migrationBuilder.AlterColumn<string>(
                name: "FK_DoctorId",
                table: "DoctorDoOperations",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "FK_AdminId",
                table: "DoctorDoOperations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DoctorDoOperations",
                table: "DoctorDoOperations",
                columns: new[] { "FK_PatientId", "FK_OperationId", "Date" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DoctorDoOperations",
                table: "DoctorDoOperations");

            migrationBuilder.DropColumn(
                name: "FK_AdminId",
                table: "DoctorDoOperations");

            migrationBuilder.AlterColumn<string>(
                name: "FK_DoctorId",
                table: "DoctorDoOperations",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DoctorDoOperations",
                table: "DoctorDoOperations",
                columns: new[] { "FK_PatientId", "FK_DoctorId", "FK_OperationId", "Date" });
        }
    }
}
