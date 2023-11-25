using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EOTools.Migrations
{
    /// <inheritdoc />
    public partial class shipClass2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShipClassId",
                table: "Ships",
                newName: "ShipClassId1");

            migrationBuilder.CreateIndex(
                name: "IX_Ships_ShipClassId1",
                table: "Ships",
                column: "ShipClassId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Ships_ShipClass_ShipClassId1",
                table: "Ships",
                column: "ShipClassId1",
                principalTable: "ShipClass",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ships_ShipClass_ShipClassId1",
                table: "Ships");

            migrationBuilder.DropIndex(
                name: "IX_Ships_ShipClassId1",
                table: "Ships");

            migrationBuilder.RenameColumn(
                name: "ShipClassId1",
                table: "Ships",
                newName: "ShipClassId");
        }
    }
}
