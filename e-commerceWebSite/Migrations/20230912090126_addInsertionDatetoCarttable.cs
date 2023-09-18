using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_commerceWebSite.Migrations
{
    public partial class addInsertionDatetoCarttable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "InsertionData",
                table: "TbCarts",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InsertionData",
                table: "TbCarts");
        }
    }
}
