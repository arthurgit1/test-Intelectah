using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Intelectah.Migrations
{
    /// <inheritdoc />
    public partial class AddAtivoToVeiculoClienteVenda : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "Vendas",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "Veiculos",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "Clientes",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "Vendas");

            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "Veiculos");

            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "Clientes");
        }
    }
}
