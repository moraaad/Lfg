using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LFG.Migrations
{
    /// <inheritdoc />
    public partial class Added_Ordine : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppOrdini",
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
                    DataOrdine = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Stato = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ImportoTotale = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IndSpedizione = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MetodoPagamento = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    ClienteId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppOrdini", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppOrdini_AppClienti_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "AppClienti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppOrdini_ClienteId",
                table: "AppOrdini",
                column: "ClienteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppOrdini");
        }
    }
}
