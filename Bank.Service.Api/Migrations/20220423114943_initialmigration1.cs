using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bank.Service.Api.Migrations
{
    public partial class initialmigration1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ShortCode",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShortCode",
                table: "Users");
        }
    }
}
