using Microsoft.Extensions.Logging;
using Tech.Challenge.Domain.Core;
using Tech.Challenge.Domain.Interfaces;
using Tech.Challenge.Domain.Interfaces.Repositories;

namespace Tech.Challenge.Application.Services.Administrativo.Produtos.AtualizarProduto;

public class AtualizarProdutoService(
    ILogger<AtualizarProdutoService> Logger,
    IProdutoRepository ProdutoRepository,
    IUnitOfWork UnitOfWork)
{
    public async Task<Result> Execute(Request request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Iniciando o processo de atualização do produto.");

        var produto = await ProdutoRepository.GetByIdAsync(request.Id, cancellationToken);

        if (produto is null)
        {
            Logger.LogWarning($"Produto com ID {request.Id} não encontrado.");
            return Result.Failure(new Exception($"Produto com ID {request.Id} não encontrado."));
        }

        var result = produto.Atualizar(request.Nome, request.Quantidade, request.PrecoUnitario, request.Tipo, request.UnidadeMedida);

        if (result.IsFailure)
            return Result.Failure(result.Error!);

        await ProdutoRepository.UpdateAsync(produto, cancellationToken);

        await UnitOfWork.SaveChangesAsync(cancellationToken);

        Logger.LogInformation($"Produto com ID {request.Id} atualizado com sucesso.");

        return Result.Success();
    }
}
