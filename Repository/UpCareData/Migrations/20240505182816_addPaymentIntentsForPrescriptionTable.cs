using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.UpCareData.Migrations
{
    public partial class addPaymentIntentsForPrescriptionTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CheckupClientSecret",
                table: "Prescriptions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CheckupPaymentIntentId",
                table: "Prescriptions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MedicineClientSecret",
                table: "Prescriptions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MedicinePaymentIntentId",
                table: "Prescriptions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrescriptionClientSecret",
                table: "Prescriptions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrescriptionPaymentIntentId",
                table: "Prescriptions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RadiologyClientSecret",
                table: "Prescriptions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RadiologyPaymentIntentId",
                table: "Prescriptions",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CheckupClientSecret",
                table: "Prescriptions");

            migrationBuilder.DropColumn(
                name: "CheckupPaymentIntentId",
                table: "Prescriptions");

            migrationBuilder.DropColumn(
                name: "MedicineClientSecret",
                table: "Prescriptions");

            migrationBuilder.DropColumn(
                name: "MedicinePaymentIntentId",
                table: "Prescriptions");

            migrationBuilder.DropColumn(
                name: "PrescriptionClientSecret",
                table: "Prescriptions");

            migrationBuilder.DropColumn(
                name: "PrescriptionPaymentIntentId",
                table: "Prescriptions");

            migrationBuilder.DropColumn(
                name: "RadiologyClientSecret",
                table: "Prescriptions");

            migrationBuilder.DropColumn(
                name: "RadiologyPaymentIntentId",
                table: "Prescriptions");
        }
    }
}
