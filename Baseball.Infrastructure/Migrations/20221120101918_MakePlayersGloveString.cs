using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Baseball.Infrastructure.Migrations
{
    public partial class MakePlayersGloveString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_Gloves_GloveId",
                table: "Players");

            migrationBuilder.DropTable(
                name: "Gloves");

            migrationBuilder.DropIndex(
                name: "IX_Players_GloveId",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "GloveId",
                table: "Players");

            migrationBuilder.AddColumn<string>(
                name: "Glove",
                table: "Players",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Glove",
                table: "Players");

            migrationBuilder.AddColumn<int>(
                name: "GloveId",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Gloves",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Brand = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Color = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Size = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gloves", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Players_GloveId",
                table: "Players",
                column: "GloveId");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Gloves_GloveId",
                table: "Players",
                column: "GloveId",
                principalTable: "Gloves",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
