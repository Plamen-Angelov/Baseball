using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Baseball.Infrastructure.Migrations
{
    public partial class AddChampionShipConnectionInTeamTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teams_ChampionShips_ChampionShipId",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Teams_ChampionShipId",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "ChampionShipId",
                table: "Teams");

            migrationBuilder.CreateTable(
                name: "ChampionShipTeam",
                columns: table => new
                {
                    ChampionShipsId = table.Column<int>(type: "int", nullable: false),
                    TeamsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChampionShipTeam", x => new { x.ChampionShipsId, x.TeamsId });
                    table.ForeignKey(
                        name: "FK_ChampionShipTeam_ChampionShips_ChampionShipsId",
                        column: x => x.ChampionShipsId,
                        principalTable: "ChampionShips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChampionShipTeam_Teams_TeamsId",
                        column: x => x.TeamsId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChampionShipTeam_TeamsId",
                table: "ChampionShipTeam",
                column: "TeamsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChampionShipTeam");

            migrationBuilder.AddColumn<int>(
                name: "ChampionShipId",
                table: "Teams",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teams_ChampionShipId",
                table: "Teams",
                column: "ChampionShipId");

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_ChampionShips_ChampionShipId",
                table: "Teams",
                column: "ChampionShipId",
                principalTable: "ChampionShips",
                principalColumn: "Id");
        }
    }
}
