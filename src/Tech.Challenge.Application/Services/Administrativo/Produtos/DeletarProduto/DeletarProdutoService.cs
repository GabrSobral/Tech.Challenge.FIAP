using Microsoft.Extensions.Logging;
using Tech.Challenge.Domain.Core;
using Tech.Challenge.Domain.Interfaces.Repositories;

namespace Tech.Challenge.Application.Services.Administrativo.Produtos.DeletarProduto;

public class DeletarProdutoService(
    ILogger<DeletarProdutoService> Logger,
    IProdutoRepository ProdutoRepository)
{
    public async Task<Result> Execute(Request request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Iniciando o processo de deleção do produto.");

        var produto = await ProdutoRepository.GetByIdAsync(request.ProdutoId, cancellationToken);

        if (produto is null)
        {
            Logger.LogWarning($"Produto com ID {request.ProdutoId} não encontrado.");
            return Result.Failure(new Exception($"Produto com ID {request.ProdutoId} não encontrado."));
        }
        await ProdutoRepository.DeleteByIdAsync(produto.Id, cancellationToken);

        Logger.LogInformation($"Produto com ID {request.ProdutoId} deletado com sucesso.");
        
        return Result.Success();
    }
}
