using Microsoft.Extensions.Logging;
using Tech.Challenge.Domain.Core;
using Tech.Challenge.Domain.Interfaces.Repositories;

namespace Tech.Challenge.Application.Services.Administrativo.Produtos.ListarProdutos;

public class ListarProdutosService(
    ILogger<ListarProdutosService> Logger,
    IProdutoRepository ProdutoRepository)
{
    public async Task<Result<List<Response>>> Execute(Request request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Iniciando o processo de listagem de produtos.");

        var produtos = await ProdutoRepository.ListAsync(request.Page ?? 1, request.Take ?? 50, cancellationToken);

        var response = produtos.Select(p => new Response
        (
            p.Id,
            p.Nome,
            p.Quantidade,
            p.PrecoUnitario,
            p.Tipo.ToString(),
            p.UnidadeMedida.ToString()
        )).ToList();

        Logger.LogInformation($"Total de {response.Count} produtos encontrados.");

        return Result.Success(response);
    }
}
