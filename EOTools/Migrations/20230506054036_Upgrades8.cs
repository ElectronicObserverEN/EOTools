using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EOTools.Migrations
{
    /// <inheritdoc />
    public partial class Upgrades8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpgradeData",
                table: "Equipments");

            migrationBuilder.AlterColumn<int>(
                name: "EquipmentUpgradeImprovmentModelId",
                table: "EquipmentUpgradeHelpersModel",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "EquipmentUpgradeImprovmentModelId",
                table: "EquipmentUpgradeHelpersModel",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<string>(
                name: "UpgradeData",
                table: "Equipments",
                type: "TEXT",
                nullable: true);
        }
    }
}
