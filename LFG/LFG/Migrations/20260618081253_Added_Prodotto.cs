using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LFG.Migrations
{
    /// <inheritdoc />
    public partial class Added_Prodotto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppProdotti",
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
                    Nome = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Descrizione = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Prezzo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CodiceSku = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Sezione = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    CategoriaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppProdotti", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppProdotti_AppCategorie_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "AppCategorie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppProdotti_CategoriaId",
                table: "AppProdotti",
                column: "CategoriaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppProdotti");
        }
    }
}
