using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EOTools.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Updates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UpdateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    WasLiveUpdate = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Updates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ApiId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    StartOnUpdateId = table.Column<int>(type: "INTEGER", nullable: true),
                    EndOnUpdateId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_Updates_EndOnUpdateId",
                        column: x => x.EndOnUpdateId,
                        principalTable: "Updates",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Events_Updates_StartOnUpdateId",
                        column: x => x.StartOnUpdateId,
                        principalTable: "Updates",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Seasons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    AddedOnUpdateId = table.Column<int>(type: "INTEGER", nullable: true),
                    RemovedOnUpdateId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seasons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Seasons_Updates_AddedOnUpdateId",
                        column: x => x.AddedOnUpdateId,
                        principalTable: "Updates",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Seasons_Updates_RemovedOnUpdateId",
                        column: x => x.RemovedOnUpdateId,
                        principalTable: "Updates",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Quests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ApiId = table.Column<int>(type: "INTEGER", nullable: false),
                    Code = table.Column<string>(type: "TEXT", nullable: false),
                    NameJP = table.Column<string>(type: "TEXT", nullable: false),
                    NameEN = table.Column<string>(type: "TEXT", nullable: false),
                    DescJP = table.Column<string>(type: "TEXT", nullable: false),
                    DescEN = table.Column<string>(type: "TEXT", nullable: false),
                    AddedOnUpdateId = table.Column<int>(type: "INTEGER", nullable: true),
                    RemovedOnUpdateId = table.Column<int>(type: "INTEGER", nullable: true),
                    SeasonId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Quests_Seasons_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "Seasons",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Quests_Updates_AddedOnUpdateId",
                        column: x => x.AddedOnUpdateId,
                        principalTable: "Updates",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Quests_Updates_RemovedOnUpdateId",
                        column: x => x.RemovedOnUpdateId,
                        principalTable: "Updates",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_EndOnUpdateId",
                table: "Events",
                column: "EndOnUpdateId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_StartOnUpdateId",
                table: "Events",
                column: "StartOnUpdateId");

            migrationBuilder.CreateIndex(
                name: "IX_Quests_AddedOnUpdateId",
                table: "Quests",
                column: "AddedOnUpdateId");

            migrationBuilder.CreateIndex(
                name: "IX_Quests_RemovedOnUpdateId",
                table: "Quests",
                column: "RemovedOnUpdateId");

            migrationBuilder.CreateIndex(
                name: "IX_Quests_SeasonId",
                table: "Quests",
                column: "SeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_Seasons_AddedOnUpdateId",
                table: "Seasons",
                column: "AddedOnUpdateId");

            migrationBuilder.CreateIndex(
                name: "IX_Seasons_RemovedOnUpdateId",
                table: "Seasons",
                column: "RemovedOnUpdateId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Quests");

            migrationBuilder.DropTable(
                name: "Seasons");

            migrationBuilder.DropTable(
                name: "Updates");
        }
    }
}
