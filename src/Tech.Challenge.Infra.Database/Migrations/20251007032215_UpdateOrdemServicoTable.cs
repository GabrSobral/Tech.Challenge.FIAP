using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tech.Challenge.Infra.Database.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrdemServicoTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletadoEm",
                table: "OrdemServicos",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletadoEm",
                table: "OrdemServicos");
        }
    }
}
