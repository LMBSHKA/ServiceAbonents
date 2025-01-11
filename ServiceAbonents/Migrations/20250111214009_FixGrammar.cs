using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceAbonents.Migrations
{
    /// <inheritdoc />
    public partial class FixGrammar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TarrifId",
                table: "Abonents",
                newName: "TarriffId");

            migrationBuilder.RenameColumn(
                name: "TarifCost",
                table: "Abonents",
                newName: "TariffCost");

            migrationBuilder.RenameColumn(
                name: "PasportData",
                table: "Abonents",
                newName: "Pasport");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TarriffId",
                table: "Abonents",
                newName: "TarrifId");

            migrationBuilder.RenameColumn(
                name: "TariffCost",
                table: "Abonents",
                newName: "TarifCost");

            migrationBuilder.RenameColumn(
                name: "Pasport",
                table: "Abonents",
                newName: "PasportData");
        }
    }
}
