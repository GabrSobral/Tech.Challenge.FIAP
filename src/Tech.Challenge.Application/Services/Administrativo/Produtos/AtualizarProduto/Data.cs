using Tech.Challenge.Domain.Enums;

namespace Tech.Challenge.Application.Services.Administrativo.Produtos.AtualizarProduto;

public record Request(
    Guid Id,
    string Nome,
    decimal PrecoUnitario,
    uint Quantidade,
    ETipoProduto Tipo,
    EUnidadeMedida UnidadeMedida);

public record RequestBody(
    string Nome,
    decimal PrecoUnitario,
    uint Quantidade,
    ETipoProduto Tipo,
    EUnidadeMedida UnidadeMedida
);
