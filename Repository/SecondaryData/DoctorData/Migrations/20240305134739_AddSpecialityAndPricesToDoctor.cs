using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.DoctorData.Migrations
{
    public partial class AddSpecialityAndPricesToDoctor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AppointmentPrice",
                table: "AspNetUsers",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ConsultationPrice",
                table: "AspNetUsers",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "IsSurgeon",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Speciality",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppointmentPrice",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ConsultationPrice",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsSurgeon",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Speciality",
                table: "AspNetUsers");
        }
    }
}
