using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.UpCareData.Migrations
{
    public partial class addColumnPatientConsultation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PaymentIntentId",
                table: "PatientConsultations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentIntentId",
                table: "PatientConsultations");
        }
    }
}
