using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceAbonents.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRemain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "LongDistanceCall",
                table: "Remains",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "UnlimMusic",
                table: "Remains",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "UnlimSocials",
                table: "Remains",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "UnlimVideo",
                table: "Remains",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LongDistanceCall",
                table: "Remains");

            migrationBuilder.DropColumn(
                name: "UnlimMusic",
                table: "Remains");

            migrationBuilder.DropColumn(
                name: "UnlimSocials",
                table: "Remains");

            migrationBuilder.DropColumn(
                name: "UnlimVideo",
                table: "Remains");
        }
    }
}
