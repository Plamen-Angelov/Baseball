using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Baseball.Infrastructure.Migrations
{
    public partial class AddBatMaterialToBat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bats_BatMaterials_MaterialId",
                table: "Bats");

            migrationBuilder.RenameColumn(
                name: "MaterialId",
                table: "Bats",
                newName: "BatMaterialId");

            migrationBuilder.RenameIndex(
                name: "IX_Bats_MaterialId",
                table: "Bats",
                newName: "IX_Bats_BatMaterialId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bats_BatMaterials_BatMaterialId",
                table: "Bats",
                column: "BatMaterialId",
                principalTable: "BatMaterials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bats_BatMaterials_BatMaterialId",
                table: "Bats");

            migrationBuilder.RenameColumn(
                name: "BatMaterialId",
                table: "Bats",
                newName: "MaterialId");

            migrationBuilder.RenameIndex(
                name: "IX_Bats_BatMaterialId",
                table: "Bats",
                newName: "IX_Bats_MaterialId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bats_BatMaterials_MaterialId",
                table: "Bats",
                column: "MaterialId",
                principalTable: "BatMaterials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
