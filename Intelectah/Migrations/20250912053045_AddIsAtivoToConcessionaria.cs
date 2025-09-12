using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Intelectah.Migrations
{
    /// <inheritdoc />
    public partial class AddIsAtivoToConcessionaria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAtivo",
                table: "Concessionarias",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAtivo",
                table: "Concessionarias");
        }
    }
}
