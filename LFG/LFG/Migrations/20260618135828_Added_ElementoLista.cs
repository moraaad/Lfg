using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LFG.Migrations
{
    /// <inheritdoc />
    public partial class Added_ElementoLista : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppElementoListe",
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
                    DataAggiunta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VarianteProdottoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ListaDesideriId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppElementoListe", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppElementoListe_AppListeDesideri_ListaDesideriId",
                        column: x => x.ListaDesideriId,
                        principalTable: "AppListeDesideri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_AppElementoListe_AppVarianteProdotti_VarianteProdottoId",
                        column: x => x.VarianteProdottoId,
                        principalTable: "AppVarianteProdotti",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppElementoListe_ListaDesideriId",
                table: "AppElementoListe",
                column: "ListaDesideriId");

            migrationBuilder.CreateIndex(
                name: "IX_AppElementoListe_VarianteProdottoId",
                table: "AppElementoListe",
                column: "VarianteProdottoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppElementoListe");
        }
    }
}
