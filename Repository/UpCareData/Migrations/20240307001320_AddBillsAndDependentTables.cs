using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.UpCareData.Migrations
{
    public partial class AddBillsAndDependentTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeliveredService = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaidMoney = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bills", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CheckupInBills",
                columns: table => new
                {
                    FK_BillId = table.Column<int>(type: "int", nullable: false),
                    FK_CheckupId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckupInBills", x => new { x.FK_CheckupId, x.FK_BillId });
                });

            migrationBuilder.CreateTable(
                name: "MedicineInBills",
                columns: table => new
                {
                    FK_MedicineId = table.Column<int>(type: "int", nullable: false),
                    FK_BillId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicineInBills", x => new { x.FK_MedicineId, x.FK_BillId });
                });

            migrationBuilder.CreateTable(
                name: "RadiologyInBill",
                columns: table => new
                {
                    FK_RadiologyId = table.Column<int>(type: "int", nullable: false),
                    FK_BillId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RadiologyInBill", x => new { x.FK_RadiologyId, x.FK_BillId });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bills");

            migrationBuilder.DropTable(
                name: "CheckupInBills");

            migrationBuilder.DropTable(
                name: "MedicineInBills");

            migrationBuilder.DropTable(
                name: "RadiologyInBill");
        }
    }
}
