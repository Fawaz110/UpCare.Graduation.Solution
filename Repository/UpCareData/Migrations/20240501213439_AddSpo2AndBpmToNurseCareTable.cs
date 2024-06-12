using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.UpCareData.Migrations
{
    public partial class AddSpo2AndBpmToNurseCareTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BeatPerMinute",
                table: "NurseCares",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OxygenSaturation",
                table: "NurseCares",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BeatPerMinute",
                table: "NurseCares");

            migrationBuilder.DropColumn(
                name: "OxygenSaturation",
                table: "NurseCares");
        }
    }
}
