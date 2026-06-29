using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LFG.Migrations
{
    /// <inheritdoc />
    public partial class Updated_Ordine_26061815033706 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ScontoId",
                table: "AppOrdini",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppOrdini_ScontoId",
                table: "AppOrdini",
                column: "ScontoId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppOrdini_AppSconti_ScontoId",
                table: "AppOrdini",
                column: "ScontoId",
                principalTable: "AppSconti",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppOrdini_AppSconti_ScontoId",
                table: "AppOrdini");

            migrationBuilder.DropIndex(
                name: "IX_AppOrdini_ScontoId",
                table: "AppOrdini");

            migrationBuilder.DropColumn(
                name: "ScontoId",
                table: "AppOrdini");
        }
    }
}
