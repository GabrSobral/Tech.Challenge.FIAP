using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tech.Challenge.Infra.Database.Migrations
{
    /// <inheritdoc />
    public partial class CreateDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Cpf = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    Nome = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    AdicionadoEm = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AtuazadoEm = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Produtos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "text", nullable: false),
                    Quantidade = table.Column<long>(type: "bigint", nullable: false),
                    PrecoUnitario = table.Column<decimal>(type: "numeric", nullable: false),
                    Tipo = table.Column<string>(type: "text", nullable: false),
                    UnidadeMedida = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produtos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Servicos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "text", nullable: false),
                    PrecoServico = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Servicos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrdemServicos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ClienteId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClienteCpf = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    CriadaEm = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AtualizadaEm = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EntregueEm = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Orcamento = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdemServicos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrdemServicos_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Veiculos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Placa = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    Modelo = table.Column<string>(type: "text", nullable: false),
                    ClienteId = table.Column<Guid>(type: "uuid", nullable: false),
                    Ano = table.Column<long>(type: "bigint", nullable: false),
                    AdicionadoEm = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Veiculos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Veiculos_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProdutosOrdemServico",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantidade = table.Column<long>(type: "bigint", nullable: false),
                    PrecoUnitario = table.Column<decimal>(type: "numeric", nullable: false),
                    ProdutoId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrdemServicoId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdutosOrdemServico", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProdutosOrdemServico_OrdemServicos_OrdemServicoId",
                        column: x => x.OrdemServicoId,
                        principalTable: "OrdemServicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProdutosOrdemServico_Produtos_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produtos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServicosOrdemServico",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ServicoId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrdemServicoId = table.Column<Guid>(type: "uuid", nullable: false),
                    PrecoServico = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServicosOrdemServico", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServicosOrdemServico_OrdemServicos_OrdemServicoId",
                        column: x => x.OrdemServicoId,
                        principalTable: "OrdemServicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServicosOrdemServico_Servicos_ServicoId",
                        column: x => x.ServicoId,
                        principalTable: "Servicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrdemServicos_ClienteId",
                table: "OrdemServicos",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_ProdutosOrdemServico_OrdemServicoId",
                table: "ProdutosOrdemServico",
                column: "OrdemServicoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProdutosOrdemServico_ProdutoId",
                table: "ProdutosOrdemServico",
                column: "ProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_ServicosOrdemServico_OrdemServicoId",
                table: "ServicosOrdemServico",
                column: "OrdemServicoId");

            migrationBuilder.CreateIndex(
                name: "IX_ServicosOrdemServico_ServicoId",
                table: "ServicosOrdemServico",
                column: "ServicoId");

            migrationBuilder.CreateIndex(
                name: "IX_Veiculos_ClienteId",
                table: "Veiculos",
                column: "ClienteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProdutosOrdemServico");

            migrationBuilder.DropTable(
                name: "ServicosOrdemServico");

            migrationBuilder.DropTable(
                name: "Veiculos");

            migrationBuilder.DropTable(
                name: "Produtos");

            migrationBuilder.DropTable(
                name: "OrdemServicos");

            migrationBuilder.DropTable(
                name: "Servicos");

            migrationBuilder.DropTable(
                name: "Clientes");
        }
    }
}
