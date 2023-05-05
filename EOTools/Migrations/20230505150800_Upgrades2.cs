using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EOTools.Migrations
{
    /// <inheritdoc />
    public partial class Upgrades2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "EquipmentUpgradeConversionModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdEquipmentAfter = table.Column<int>(type: "INTEGER", nullable: false),
                    EquipmentLevelAfter = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentUpgradeConversionModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentUpgradeImprovmentCostDetail",
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
                    table.PrimaryKey("PK_EquipmentUpgradeImprovmentCostDetail", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentUpgrades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EquipmentId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentUpgrades", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentUpgradeImprovmentCost",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Fuel = table.Column<int>(type: "INTEGER", nullable: false),
                    Ammo = table.Column<int>(type: "INTEGER", nullable: false),
                    Steel = table.Column<int>(type: "INTEGER", nullable: false),
                    Bauxite = table.Column<int>(type: "INTEGER", nullable: false),
                    Cost0To5Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Cost6To9Id = table.Column<int>(type: "INTEGER", nullable: false),
                    CostMaxId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentUpgradeImprovmentCost", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentUpgradeImprovmentCost_EquipmentUpgradeImprovmentCostDetail_Cost0To5Id",
                        column: x => x.Cost0To5Id,
                        principalTable: "EquipmentUpgradeImprovmentCostDetail",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EquipmentUpgradeImprovmentCost_EquipmentUpgradeImprovmentCostDetail_Cost6To9Id",
                        column: x => x.Cost6To9Id,
                        principalTable: "EquipmentUpgradeImprovmentCostDetail",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EquipmentUpgradeImprovmentCost_EquipmentUpgradeImprovmentCostDetail_CostMaxId",
                        column: x => x.CostMaxId,
                        principalTable: "EquipmentUpgradeImprovmentCostDetail",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EquipmentUpgradeImprovmentCostItemDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ItemId = table.Column<int>(type: "INTEGER", nullable: false),
                    Count = table.Column<int>(type: "INTEGER", nullable: false),
                    EquipmentUpgradeImprovmentCostDetailId = table.Column<int>(type: "INTEGER", nullable: true),
                    EquipmentUpgradeImprovmentCostDetailId1 = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentUpgradeImprovmentCostItemDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentUpgradeImprovmentCostItemDetail_EquipmentUpgradeImprovmentCostDetail_EquipmentUpgradeImprovmentCostDetailId",
                        column: x => x.EquipmentUpgradeImprovmentCostDetailId,
                        principalTable: "EquipmentUpgradeImprovmentCostDetail",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EquipmentUpgradeImprovmentCostItemDetail_EquipmentUpgradeImprovmentCostDetail_EquipmentUpgradeImprovmentCostDetailId1",
                        column: x => x.EquipmentUpgradeImprovmentCostDetailId1,
                        principalTable: "EquipmentUpgradeImprovmentCostDetail",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EquipmentUpgradeImprovmentModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ConversionDataId = table.Column<int>(type: "INTEGER", nullable: true),
                    CostsId = table.Column<int>(type: "INTEGER", nullable: false),
                    EquipmentUpgradeDataModelId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentUpgradeImprovmentModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentUpgradeImprovmentModel_EquipmentUpgradeConversionModel_ConversionDataId",
                        column: x => x.ConversionDataId,
                        principalTable: "EquipmentUpgradeConversionModel",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EquipmentUpgradeImprovmentModel_EquipmentUpgradeImprovmentCost_CostsId",
                        column: x => x.CostsId,
                        principalTable: "EquipmentUpgradeImprovmentCost",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EquipmentUpgradeImprovmentModel_EquipmentUpgrades_EquipmentUpgradeDataModelId",
                        column: x => x.EquipmentUpgradeDataModelId,
                        principalTable: "EquipmentUpgrades",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EquipmentUpgradeHelpersModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EquipmentUpgradeImprovmentModelId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentUpgradeHelpersModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentUpgradeHelpersModel_EquipmentUpgradeImprovmentModel_EquipmentUpgradeImprovmentModelId",
                        column: x => x.EquipmentUpgradeImprovmentModelId,
                        principalTable: "EquipmentUpgradeImprovmentModel",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EquipmentUpgradeHelpersDayModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Day = table.Column<int>(type: "INTEGER", nullable: false),
                    EquipmentUpgradeHelpersModelId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentUpgradeHelpersDayModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentUpgradeHelpersDayModel_EquipmentUpgradeHelpersModel_EquipmentUpgradeHelpersModelId",
                        column: x => x.EquipmentUpgradeHelpersModelId,
                        principalTable: "EquipmentUpgradeHelpersModel",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EquipmentUpgradeHelpersShipModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ShipId = table.Column<int>(type: "INTEGER", nullable: false),
                    EquipmentUpgradeHelpersModelId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentUpgradeHelpersShipModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentUpgradeHelpersShipModel_EquipmentUpgradeHelpersModel_EquipmentUpgradeHelpersModelId",
                        column: x => x.EquipmentUpgradeHelpersModelId,
                        principalTable: "EquipmentUpgradeHelpersModel",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentUpgradeHelpersDayModel_EquipmentUpgradeHelpersModelId",
                table: "EquipmentUpgradeHelpersDayModel",
                column: "EquipmentUpgradeHelpersModelId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentUpgradeHelpersModel_EquipmentUpgradeImprovmentModelId",
                table: "EquipmentUpgradeHelpersModel",
                column: "EquipmentUpgradeImprovmentModelId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentUpgradeHelpersShipModel_EquipmentUpgradeHelpersModelId",
                table: "EquipmentUpgradeHelpersShipModel",
                column: "EquipmentUpgradeHelpersModelId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentUpgradeImprovmentCost_Cost0To5Id",
                table: "EquipmentUpgradeImprovmentCost",
                column: "Cost0To5Id");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentUpgradeImprovmentCost_Cost6To9Id",
                table: "EquipmentUpgradeImprovmentCost",
                column: "Cost6To9Id");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentUpgradeImprovmentCost_CostMaxId",
                table: "EquipmentUpgradeImprovmentCost",
                column: "CostMaxId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentUpgradeImprovmentCostItemDetail_EquipmentUpgradeImprovmentCostDetailId",
                table: "EquipmentUpgradeImprovmentCostItemDetail",
                column: "EquipmentUpgradeImprovmentCostDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentUpgradeImprovmentCostItemDetail_EquipmentUpgradeImprovmentCostDetailId1",
                table: "EquipmentUpgradeImprovmentCostItemDetail",
                column: "EquipmentUpgradeImprovmentCostDetailId1");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentUpgradeImprovmentModel_ConversionDataId",
                table: "EquipmentUpgradeImprovmentModel",
                column: "ConversionDataId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentUpgradeImprovmentModel_CostsId",
                table: "EquipmentUpgradeImprovmentModel",
                column: "CostsId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentUpgradeImprovmentModel_EquipmentUpgradeDataModelId",
                table: "EquipmentUpgradeImprovmentModel",
                column: "EquipmentUpgradeDataModelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquipmentUpgradeHelpersDayModel");

            migrationBuilder.DropTable(
                name: "EquipmentUpgradeHelpersShipModel");

            migrationBuilder.DropTable(
                name: "EquipmentUpgradeImprovmentCostItemDetail");

            migrationBuilder.DropTable(
                name: "EquipmentUpgradeHelpersModel");

            migrationBuilder.DropTable(
                name: "EquipmentUpgradeImprovmentModel");

            migrationBuilder.DropTable(
                name: "EquipmentUpgradeConversionModel");

            migrationBuilder.DropTable(
                name: "EquipmentUpgradeImprovmentCost");

            migrationBuilder.DropTable(
                name: "EquipmentUpgrades");

            migrationBuilder.DropTable(
                name: "EquipmentUpgradeImprovmentCostDetail");

            migrationBuilder.EnsureSchema(
                name: "EquipmentUpgrades");

            migrationBuilder.CreateTable(
                name: "Convertions",
                schema: "EquipmentUpgrades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EquipmentAfterId = table.Column<int>(type: "INTEGER", nullable: false),
                    EquipmentLevelAfter = table.Column<int>(type: "INTEGER", nullable: false),
                    ImprovmentId = table.Column<int>(type: "INTEGER", nullable: false)
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
                    Count = table.Column<int>(type: "INTEGER", nullable: false),
                    ImprovmentCostDetailId = table.Column<int>(type: "INTEGER", nullable: false),
                    ItemId = table.Column<int>(type: "INTEGER", nullable: false)
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
                    Day = table.Column<int>(type: "INTEGER", nullable: false),
                    HelperGroupId = table.Column<int>(type: "INTEGER", nullable: false)
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
                    Ammo = table.Column<int>(type: "INTEGER", nullable: false),
                    Bauxite = table.Column<int>(type: "INTEGER", nullable: false),
                    Cost0To5 = table.Column<int>(type: "INTEGER", nullable: false),
                    Cost6To9 = table.Column<int>(type: "INTEGER", nullable: false),
                    CostMax = table.Column<int>(type: "INTEGER", nullable: true),
                    Fuel = table.Column<int>(type: "INTEGER", nullable: false),
                    ImprovmentId = table.Column<int>(type: "INTEGER", nullable: false),
                    Steel = table.Column<int>(type: "INTEGER", nullable: false)
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
                    ImproveMatCost = table.Column<int>(type: "INTEGER", nullable: false),
                    SliderDevmatCost = table.Column<int>(type: "INTEGER", nullable: false),
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
                    Count = table.Column<int>(type: "INTEGER", nullable: false),
                    ImprovmentCostDetailId = table.Column<int>(type: "INTEGER", nullable: false),
                    ItemId = table.Column<int>(type: "INTEGER", nullable: false)
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
    }
}
