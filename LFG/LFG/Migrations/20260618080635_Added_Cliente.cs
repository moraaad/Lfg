using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LFG.Migrations
{
    /// <inheritdoc />
    public partial class Added_Cliente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppClienti",
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
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Cognome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Genere = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataNascita = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Sezione = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Nazionalita = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppClienti", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppClienti");
        }
    }
}
