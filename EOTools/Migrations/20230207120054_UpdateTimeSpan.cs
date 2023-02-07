using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EOTools.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTimeSpan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Updates_EndOnUpdateId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Updates_StartOnUpdateId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Seasons_Updates_AddedOnUpdateId",
                table: "Seasons");

            migrationBuilder.DropForeignKey(
                name: "FK_Seasons_Updates_RemovedOnUpdateId",
                table: "Seasons");

            migrationBuilder.DropIndex(
                name: "IX_Seasons_AddedOnUpdateId",
                table: "Seasons");

            migrationBuilder.DropIndex(
                name: "IX_Seasons_RemovedOnUpdateId",
                table: "Seasons");

            migrationBuilder.DropIndex(
                name: "IX_Events_EndOnUpdateId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_StartOnUpdateId",
                table: "Events");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "UpdateEndTime",
                table: "Updates",
                type: "TEXT",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "UpdateStartTime",
                table: "Updates",
                type: "TEXT",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdateEndTime",
                table: "Updates");

            migrationBuilder.DropColumn(
                name: "UpdateStartTime",
                table: "Updates");

            migrationBuilder.CreateIndex(
                name: "IX_Seasons_AddedOnUpdateId",
                table: "Seasons",
                column: "AddedOnUpdateId");

            migrationBuilder.CreateIndex(
                name: "IX_Seasons_RemovedOnUpdateId",
                table: "Seasons",
                column: "RemovedOnUpdateId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_EndOnUpdateId",
                table: "Events",
                column: "EndOnUpdateId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_StartOnUpdateId",
                table: "Events",
                column: "StartOnUpdateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Updates_EndOnUpdateId",
                table: "Events",
                column: "EndOnUpdateId",
                principalTable: "Updates",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Updates_StartOnUpdateId",
                table: "Events",
                column: "StartOnUpdateId",
                principalTable: "Updates",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Seasons_Updates_AddedOnUpdateId",
                table: "Seasons",
                column: "AddedOnUpdateId",
                principalTable: "Updates",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Seasons_Updates_RemovedOnUpdateId",
                table: "Seasons",
                column: "RemovedOnUpdateId",
                principalTable: "Updates",
                principalColumn: "Id");
        }
    }
}
