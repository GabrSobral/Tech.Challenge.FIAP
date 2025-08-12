namespace Tech.Challenge.Application.Services.Administrativo.Produtos.ListarProdutos;

public record Request(
    int? Page,
    int? Take
);

public record Response(
    Guid Id,
    string Nome,
    uint Quantidade,
    decimal PrecoUnitario,
    string Tipo,
    string UnidadeMedida);
