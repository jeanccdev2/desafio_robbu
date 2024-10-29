using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Robbu.Desafio.Jean.API.Migrations
{
    /// <inheritdoc />
    public partial class ModelProductAddIsDeleted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "products",
                type: "boolean",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "products");
        }
    }
}
