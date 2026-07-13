using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LFG.Migrations
{
    /// <inheritdoc />
    public partial class Updated_Ordine_26070810463063 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "IndirizzoId",
                table: "AppOrdini",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppOrdini_IndirizzoId",
                table: "AppOrdini",
                column: "IndirizzoId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppOrdini_AppIndirizzi_IndirizzoId",
                table: "AppOrdini",
                column: "IndirizzoId",
                principalTable: "AppIndirizzi",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppOrdini_AppIndirizzi_IndirizzoId",
                table: "AppOrdini");

            migrationBuilder.DropIndex(
                name: "IX_AppOrdini_IndirizzoId",
                table: "AppOrdini");

            migrationBuilder.DropColumn(
                name: "IndirizzoId",
                table: "AppOrdini");
        }
    }
}
