using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LFG.Migrations
{
    /// <inheritdoc />
    public partial class Added_Recensione : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppRecensioni",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Valutazione = table.Column<int>(type: "int", maxLength: 5, nullable: false),
                    Commento = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DataRecensione = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClienteId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ProdottoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppRecensioni", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppRecensioni_AppClienti_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "AppClienti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_AppRecensioni_AppProdotti_ProdottoId",
                        column: x => x.ProdottoId,
                        principalTable: "AppProdotti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppRecensioni_ClienteId",
                table: "AppRecensioni",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_AppRecensioni_ProdottoId",
                table: "AppRecensioni",
                column: "ProdottoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppRecensioni");
        }
    }
}
