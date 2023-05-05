using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EOTools.Migrations
{
    /// <inheritdoc />
    public partial class Upgrades5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentUpgradeImprovmentModel_EquipmentUpgradeConversionModel_ConversionDataId",
                table: "EquipmentUpgradeImprovmentModel");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentUpgradeImprovmentModel_ConversionDataId",
                table: "EquipmentUpgradeImprovmentModel");

            migrationBuilder.DropColumn(
                name: "ConversionDataId",
                table: "EquipmentUpgradeImprovmentModel");

            migrationBuilder.AddColumn<int>(
                name: "ImprovmentModelId",
                table: "EquipmentUpgradeConversionModel",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentUpgradeConversionModel_ImprovmentModelId",
                table: "EquipmentUpgradeConversionModel",
                column: "ImprovmentModelId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentUpgradeConversionModel_EquipmentUpgradeImprovmentModel_ImprovmentModelId",
                table: "EquipmentUpgradeConversionModel",
                column: "ImprovmentModelId",
                principalTable: "EquipmentUpgradeImprovmentModel",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentUpgradeConversionModel_EquipmentUpgradeImprovmentModel_ImprovmentModelId",
                table: "EquipmentUpgradeConversionModel");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentUpgradeConversionModel_ImprovmentModelId",
                table: "EquipmentUpgradeConversionModel");

            migrationBuilder.DropColumn(
                name: "ImprovmentModelId",
                table: "EquipmentUpgradeConversionModel");

            migrationBuilder.AddColumn<int>(
                name: "ConversionDataId",
                table: "EquipmentUpgradeImprovmentModel",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentUpgradeImprovmentModel_ConversionDataId",
                table: "EquipmentUpgradeImprovmentModel",
                column: "ConversionDataId");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentUpgradeImprovmentModel_EquipmentUpgradeConversionModel_ConversionDataId",
                table: "EquipmentUpgradeImprovmentModel",
                column: "ConversionDataId",
                principalTable: "EquipmentUpgradeConversionModel",
                principalColumn: "Id");
        }
    }
}
