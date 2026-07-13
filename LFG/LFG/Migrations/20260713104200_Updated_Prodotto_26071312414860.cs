using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LFG.Migrations
{
    /// <inheritdoc />
    public partial class Updated_Prodotto_26071312414860 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppProdottoColleziones_AppProdotti_ProdottoId",
                table: "AppProdottoColleziones");

            migrationBuilder.AddColumn<Guid>(
                name: "CollezioneId",
                table: "AppProdotti",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppProdotti_CollezioneId",
                table: "AppProdotti",
                column: "CollezioneId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppProdotti_AppCollezioni_CollezioneId",
                table: "AppProdotti",
                column: "CollezioneId",
                principalTable: "AppCollezioni",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppProdotti_AppCollezioni_CollezioneId",
                table: "AppProdotti");

            migrationBuilder.DropIndex(
                name: "IX_AppProdotti_CollezioneId",
                table: "AppProdotti");

            migrationBuilder.DropColumn(
                name: "CollezioneId",
                table: "AppProdotti");

            migrationBuilder.AddForeignKey(
                name: "FK_AppProdottoColleziones_AppProdotti_ProdottoId",
                table: "AppProdottoColleziones",
                column: "ProdottoId",
                principalTable: "AppProdotti",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
