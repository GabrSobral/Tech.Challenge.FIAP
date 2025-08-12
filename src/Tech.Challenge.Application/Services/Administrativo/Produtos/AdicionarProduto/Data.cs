using Tech.Challenge.Domain.Enums;

namespace Tech.Challenge.Application.Services.Administrativo.Produtos.AdicionarProduto;

public record Request(
    string Nome,
    decimal PrecoUnitario,
    uint Quantidade,
    ETipoProduto Tipo,
    EUnidadeMedida UnidadeMedida
);

public record Response(Guid Id);
