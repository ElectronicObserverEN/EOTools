using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EOTools.Migrations
{
    /// <inheritdoc />
    public partial class ForeignKeysOfQuests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quests_Seasons_SeasonId",
                table: "Quests");

            migrationBuilder.DropForeignKey(
                name: "FK_Quests_Updates_AddedOnUpdateId",
                table: "Quests");

            migrationBuilder.DropForeignKey(
                name: "FK_Quests_Updates_RemovedOnUpdateId",
                table: "Quests");

            migrationBuilder.DropIndex(
                name: "IX_Quests_AddedOnUpdateId",
                table: "Quests");

            migrationBuilder.DropIndex(
                name: "IX_Quests_RemovedOnUpdateId",
                table: "Quests");

            migrationBuilder.DropIndex(
                name: "IX_Quests_SeasonId",
                table: "Quests");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddForeignKey(
                name: "FK_Quests_Seasons_SeasonId",
                table: "Quests",
                column: "SeasonId",
                principalTable: "Seasons",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Quests_Updates_AddedOnUpdateId",
                table: "Quests",
                column: "AddedOnUpdateId",
                principalTable: "Updates",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Quests_Updates_RemovedOnUpdateId",
                table: "Quests",
                column: "RemovedOnUpdateId",
                principalTable: "Updates",
                principalColumn: "Id");
        }
    }
}
