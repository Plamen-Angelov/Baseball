using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Baseball.Infrastructure.Migrations
{
    public partial class CreateTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Material = table.Column<int>(type: "int", nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Size = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChampionShips",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChampionShips", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Gloves",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Brand = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Size = table.Column<int>(type: "int", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gloves", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    HomeColor = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    AwayColor = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChampionShipName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    HomeTeamName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    AwayTeamName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Stadium = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    InningPlayed = table.Column<int>(type: "int", nullable: false),
                    HomeTeamRuns = table.Column<int>(type: "int", nullable: false),
                    AwayTeamRuns = table.Column<int>(type: "int", nullable: false),
                    HomeTeamHits = table.Column<int>(type: "int", nullable: false),
                    AwayTeamHits = table.Column<int>(type: "int", nullable: false),
                    HomeTeamErrors = table.Column<int>(type: "int", nullable: false),
                    AwayTeamErrors = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ChampionShipId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Games_ChampionShips_ChampionShipId",
                        column: x => x.ChampionShipId,
                        principalTable: "ChampionShips",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TeamResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeamName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    WinGames = table.Column<int>(type: "int", nullable: false),
                    LoseGames = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ChampionShipId = table.Column<int>(type: "int", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Position = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: false),
                    BatId = table.Column<int>(type: "int", nullable: false),
                    GloveId = table.Column<int>(type: "int", nullable: false),
                    ThrowHand = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BattingAverage = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Players_Bats_BatId",
                        column: x => x.BatId,
                        principalTable: "Bats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Players_Gloves_GloveId",
                        column: x => x.GloveId,
                        principalTable: "Gloves",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Games_ChampionShipId",
                table: "Games",
                column: "ChampionShipId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_BatId",
                table: "Players",
                column: "BatId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_GloveId",
                table: "Players",
                column: "GloveId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamResults_ChampionShipId",
                table: "TeamResults",
                column: "ChampionShipId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "TeamResults");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "Bats");

            migrationBuilder.DropTable(
                name: "Gloves");

            migrationBuilder.DropTable(
                name: "ChampionShips");
        }
    }
}
