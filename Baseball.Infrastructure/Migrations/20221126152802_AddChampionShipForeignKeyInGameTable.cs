using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Baseball.Infrastructure.Migrations
{
    public partial class AddChampionShipForeignKeyInGameTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_ChampionShips_ChampionShipId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "ChampionShipName",
                table: "Games");

            migrationBuilder.AlterColumn<int>(
                name: "ChampionShipId",
                table: "Games",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_ChampionShips_ChampionShipId",
                table: "Games",
                column: "ChampionShipId",
                principalTable: "ChampionShips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_ChampionShips_ChampionShipId",
                table: "Games");

            migrationBuilder.AlterColumn<int>(
                name: "ChampionShipId",
                table: "Games",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "ChampionShipName",
                table: "Games",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_ChampionShips_ChampionShipId",
                table: "Games",
                column: "ChampionShipId",
                principalTable: "ChampionShips",
                principalColumn: "Id");
        }
    }
}
