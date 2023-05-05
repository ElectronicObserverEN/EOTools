using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EOTools.Migrations
{
    /// <inheritdoc />
    public partial class Upgrades : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "EquipmentUpgrades");

            migrationBuilder.CreateTable(
                name: "Convertions",
                schema: "EquipmentUpgrades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ImprovmentId = table.Column<int>(type: "INTEGER", nullable: false),
                    EquipmentAfterId = table.Column<int>(type: "INTEGER", nullable: false),
                    EquipmentLevelAfter = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Convertions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentCostDetail",
                schema: "EquipmentUpgrades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ImprovmentCostDetailId = table.Column<int>(type: "INTEGER", nullable: false),
                    ItemId = table.Column<int>(type: "INTEGER", nullable: false),
                    Count = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentCostDetail", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HelperGroup",
                schema: "EquipmentUpgrades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ImprovmentId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HelperGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HelperGroupDay",
                schema: "EquipmentUpgrades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    HelperGroupId = table.Column<int>(type: "INTEGER", nullable: false),
                    Day = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HelperGroupDay", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HelperShip",
                schema: "EquipmentUpgrades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    HelperGroupId = table.Column<int>(type: "INTEGER", nullable: false),
                    ShipId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HelperShip", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Improvment",
                schema: "EquipmentUpgrades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdEquipment = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Improvment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ImprovmentCost",
                schema: "EquipmentUpgrades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ImprovmentId = table.Column<int>(type: "INTEGER", nullable: false),
                    Fuel = table.Column<int>(type: "INTEGER", nullable: false),
                    Ammo = table.Column<int>(type: "INTEGER", nullable: false),
                    Steel = table.Column<int>(type: "INTEGER", nullable: false),
                    Bauxite = table.Column<int>(type: "INTEGER", nullable: false),
                    Cost0To5 = table.Column<int>(type: "INTEGER", nullable: false),
                    Cost6To9 = table.Column<int>(type: "INTEGER", nullable: false),
                    CostMax = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImprovmentCost", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ImprovmentCostDetail",
                schema: "EquipmentUpgrades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DevmatCost = table.Column<int>(type: "INTEGER", nullable: false),
                    SliderDevmatCost = table.Column<int>(type: "INTEGER", nullable: false),
                    ImproveMatCost = table.Column<int>(type: "INTEGER", nullable: false),
                    SliderImproveMatCost = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImprovmentCostDetail", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UseItemtCostDetail",
                schema: "EquipmentUpgrades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ImprovmentCostDetailId = table.Column<int>(type: "INTEGER", nullable: false),
                    ItemId = table.Column<int>(type: "INTEGER", nullable: false),
                    Count = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UseItemtCostDetail", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Convertions_ImprovmentId",
                schema: "EquipmentUpgrades",
                table: "Convertions",
                column: "ImprovmentId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentCostDetail_ImprovmentCostDetailId_ItemId",
                schema: "EquipmentUpgrades",
                table: "EquipmentCostDetail",
                columns: new[] { "ImprovmentCostDetailId", "ItemId" });

            migrationBuilder.CreateIndex(
                name: "IX_HelperGroupDay_HelperGroupId_Day",
                schema: "EquipmentUpgrades",
                table: "HelperGroupDay",
                columns: new[] { "HelperGroupId", "Day" });

            migrationBuilder.CreateIndex(
                name: "IX_HelperShip_HelperGroupId_ShipId",
                schema: "EquipmentUpgrades",
                table: "HelperShip",
                columns: new[] { "HelperGroupId", "ShipId" });

            migrationBuilder.CreateIndex(
                name: "IX_UseItemtCostDetail_ImprovmentCostDetailId_ItemId",
                schema: "EquipmentUpgrades",
                table: "UseItemtCostDetail",
                columns: new[] { "ImprovmentCostDetailId", "ItemId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Convertions",
                schema: "EquipmentUpgrades");

            migrationBuilder.DropTable(
                name: "EquipmentCostDetail",
                schema: "EquipmentUpgrades");

            migrationBuilder.DropTable(
                name: "HelperGroup",
                schema: "EquipmentUpgrades");

            migrationBuilder.DropTable(
                name: "HelperGroupDay",
                schema: "EquipmentUpgrades");

            migrationBuilder.DropTable(
                name: "HelperShip",
                schema: "EquipmentUpgrades");

            migrationBuilder.DropTable(
                name: "Improvment",
                schema: "EquipmentUpgrades");

            migrationBuilder.DropTable(
                name: "ImprovmentCost",
                schema: "EquipmentUpgrades");

            migrationBuilder.DropTable(
                name: "ImprovmentCostDetail",
                schema: "EquipmentUpgrades");

            migrationBuilder.DropTable(
                name: "UseItemtCostDetail",
                schema: "EquipmentUpgrades");
        }
    }
}
