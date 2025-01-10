using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceAbonents.Migrations
{
    /// <inheritdoc />
    public partial class StatusAndTarifCost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DateForDeduct",
                table: "Abonents",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateForDeduct",
                table: "Abonents");
        }
    }
}
