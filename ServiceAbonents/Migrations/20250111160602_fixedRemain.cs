using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceAbonents.Migrations
{
    /// <inheritdoc />
    public partial class fixedRemain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReaminGb",
                table: "Remains",
                newName: "RemainGb");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RemainGb",
                table: "Remains",
                newName: "ReaminGb");
        }
    }
}
