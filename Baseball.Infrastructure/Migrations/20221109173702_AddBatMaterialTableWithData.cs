using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Baseball.Infrastructure.Migrations
{
    public partial class AddBatMaterialTableWithData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Material",
                table: "Bats",
                newName: "MaterialId");

            migrationBuilder.CreateTable(
                name: "BatMaterials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BatMaterials", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "BatMaterials",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Wood" });

            migrationBuilder.InsertData(
                table: "BatMaterials",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "Aluminium" });

            migrationBuilder.CreateIndex(
                name: "IX_Bats_MaterialId",
                table: "Bats",
                column: "MaterialId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bats_BatMaterials_MaterialId",
                table: "Bats",
                column: "MaterialId",
                principalTable: "BatMaterials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bats_BatMaterials_MaterialId",
                table: "Bats");

            migrationBuilder.DropTable(
                name: "BatMaterials");

            migrationBuilder.DropIndex(
                name: "IX_Bats_MaterialId",
                table: "Bats");

            migrationBuilder.RenameColumn(
                name: "MaterialId",
                table: "Bats",
                newName: "Material");
        }
    }
}
