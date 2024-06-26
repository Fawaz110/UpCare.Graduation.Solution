﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.UpCareData.Migrations
{
    public partial class removeColumnFromCheckupInBill : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "CheckupInBills");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "CheckupInBills",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
