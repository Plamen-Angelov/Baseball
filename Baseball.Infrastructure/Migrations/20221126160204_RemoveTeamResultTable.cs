using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Baseball.Infrastructure.Migrations
{
    public partial class RemoveTeamResultTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TeamResults");

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

        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "TeamResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChampionShipId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LoseGames = table.Column<int>(type: "int", nullable: false),
                    TeamName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    WinGames = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeamResults_ChampionShips_ChampionShipId",
                        column: x => x.ChampionShipId,
                        principalTable: "ChampionShips",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TeamResults_ChampionShipId",
                table: "TeamResults",
                column: "ChampionShipId");
        }
    }
}
