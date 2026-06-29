using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LFG.Migrations
{
    /// <inheritdoc />
    public partial class Updated_Prodotto_26061815010082 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppProdottoColleziones",
                columns: table => new
                {
                    ProdottoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CollezioneId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppProdottoColleziones", x => new { x.ProdottoId, x.CollezioneId });
                    table.ForeignKey(
                        name: "FK_AppProdottoColleziones_AppCollezioni_CollezioneId",
                        column: x => x.CollezioneId,
                        principalTable: "AppCollezioni",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppProdottoColleziones_AppProdotti_ProdottoId",
                        column: x => x.ProdottoId,
                        principalTable: "AppProdotti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppProdottoColleziones_CollezioneId",
                table: "AppProdottoColleziones",
                column: "CollezioneId");

            migrationBuilder.CreateIndex(
                name: "IX_AppProdottoColleziones_ProdottoId_CollezioneId",
                table: "AppProdottoColleziones",
                columns: new[] { "ProdottoId", "CollezioneId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppProdottoColleziones");
        }
    }
}
