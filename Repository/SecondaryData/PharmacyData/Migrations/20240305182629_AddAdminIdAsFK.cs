using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.PharmacyData.Migrations
{
    public partial class AddAdminIdAsFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FK_AdminId",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FK_AdminId",
                table: "AspNetUsers");
        }
    }
}
