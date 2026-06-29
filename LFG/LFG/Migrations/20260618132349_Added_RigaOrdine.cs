using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LFG.Migrations
{
    /// <inheritdoc />
    public partial class Added_RigaOrdine : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppRigaOrdini",
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
                    Quantita = table.Column<int>(type: "int", maxLength: 300, nullable: false),
                    PrezzoUnitario = table.Column<decimal>(type: "decimal(18,2)", maxLength: 10000, nullable: false),
                    OrdineId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    VarianteProdottoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppRigaOrdini", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppRigaOrdini_AppOrdini_OrdineId",
                        column: x => x.OrdineId,
                        principalTable: "AppOrdini",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_AppRigaOrdini_AppVarianteProdotti_VarianteProdottoId",
                        column: x => x.VarianteProdottoId,
                        principalTable: "AppVarianteProdotti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppRigaOrdini_OrdineId",
                table: "AppRigaOrdini",
                column: "OrdineId");

            migrationBuilder.CreateIndex(
                name: "IX_AppRigaOrdini_VarianteProdottoId",
                table: "AppRigaOrdini",
                column: "VarianteProdottoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppRigaOrdini");
        }
    }
}
