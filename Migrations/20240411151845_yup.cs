using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace projectwerk.Migrations
{
    public partial class yup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderFulfilled",
                table: "Orders");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "OrderFulfilled",
                table: "Orders",
                type: "datetime2",
                nullable: true);
        }
    }
}
