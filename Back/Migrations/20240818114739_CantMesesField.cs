using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Back.Migrations
{
    /// <inheritdoc />
    public partial class CantMesesField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DuracionEnDias",
                table: "Membresias");

            migrationBuilder.AddColumn<double>(
                name: "DuracionEnMeses",
                table: "Membresias",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DuracionEnMeses",
                table: "Membresias");

            migrationBuilder.AddColumn<int>(
                name: "DuracionEnDias",
                table: "Membresias",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
