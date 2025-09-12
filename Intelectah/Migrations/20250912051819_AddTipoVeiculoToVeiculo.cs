using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Intelectah.Migrations
{
    /// <inheritdoc />
    public partial class AddTipoVeiculoToVeiculo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Tipo",
                table: "Veiculos",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "Veiculos");
        }
    }
}
