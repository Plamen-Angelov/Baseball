using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Baseball.Infrastructure.Migrations
{
    public partial class AddLinLoseToTeamTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WinGames",
                table: "Teams",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "loseGames",
                table: "Teams",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WinGames",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "loseGames",
                table: "Teams");
        }
    }
}
