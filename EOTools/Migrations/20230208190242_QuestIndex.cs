using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EOTools.Migrations
{
    /// <inheritdoc />
    public partial class QuestIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Tracker",
                table: "Quests",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Quests_ApiId",
                table: "Quests",
                column: "ApiId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Quests_Code",
                table: "Quests",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Quests_ApiId",
                table: "Quests");

            migrationBuilder.DropIndex(
                name: "IX_Quests_Code",
                table: "Quests");

            migrationBuilder.DropColumn(
                name: "Tracker",
                table: "Quests");
        }
    }
}
