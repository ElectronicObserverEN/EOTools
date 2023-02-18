using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EOTools.Migrations
{
    /// <inheritdoc />
    public partial class DevsTweets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EndTweetLink",
                table: "Updates",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StartTweetLink",
                table: "Updates",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndTweetLink",
                table: "Updates");

            migrationBuilder.DropColumn(
                name: "StartTweetLink",
                table: "Updates");
        }
    }
}
