using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LFG.Migrations
{
    /// <inheritdoc />
    public partial class Added_Indirizzo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppIndirizzi",
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
                    Paese = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Citta = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Provincia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Via = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Cap = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    ClienteId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppIndirizzi", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppIndirizzi_AppClienti_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "AppClienti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppIndirizzi_ClienteId",
                table: "AppIndirizzi",
                column: "ClienteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppIndirizzi");
        }
    }
}
