using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EOTools.Migrations
{
    /// <inheritdoc />
    public partial class QuestIndex2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Quests_ApiId",
                table: "Quests");

            migrationBuilder.DropIndex(
                name: "IX_Quests_Code",
                table: "Quests");

            migrationBuilder.CreateIndex(
                name: "IX_Quests_Code_ApiId",
                table: "Quests",
                columns: new[] { "Code", "ApiId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Quests_Code_ApiId",
                table: "Quests");

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
    }
}
