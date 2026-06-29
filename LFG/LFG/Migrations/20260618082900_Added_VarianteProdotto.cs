using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LFG.Migrations
{
    /// <inheritdoc />
    public partial class Added_VarianteProdotto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppVarianteProdotti",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProdottoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Taglia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Colore = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Materiale = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UrlImmagine = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    QtaMagazzino = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppVarianteProdotti", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppVarianteProdotti_AppProdotti_ProdottoId",
                        column: x => x.ProdottoId,
                        principalTable: "AppProdotti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppVarianteProdotti_ProdottoId",
                table: "AppVarianteProdotti",
                column: "ProdottoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppVarianteProdotti");
        }
    }
}
