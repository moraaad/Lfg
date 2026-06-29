using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LFG.Migrations
{
    /// <inheritdoc />
    public partial class Updated_ListaDesideri_26061815524692 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NomeLista",
                table: "AppListeDesideri",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NomeLista",
                table: "AppListeDesideri");
        }
    }
}
