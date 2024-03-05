using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.PatientData.Migrations
{
    public partial class AddReceptionistIdAsFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FK_ReceptionistId",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FK_ReceptionistId",
                table: "AspNetUsers");
        }
    }
}
