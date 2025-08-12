using Microsoft.Extensions.Logging;
using Tech.Challenge.Domain.Core;
using Tech.Challenge.Domain.Interfaces;
using Tech.Challenge.Domain.Interfaces.Repositories;

namespace Tech.Challenge.Application.Services.Administrativo.Produtos.AdicionarProduto;

public class AdicionarProdutoService(
    ILogger<AdicionarProdutoService> Logger,
    IProdutoRepository ProdutoRepository,
    IUnitOfWork UnitOfWork)
{
    public async Task<Result<Response>> Execute(Request request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Iniciando o processo de adição do produto.");
        
        var produto = Tech.Challenge.Domain.Entities.Produto.Produto.Criar(request.Nome, request.Quantidade, request.PrecoUnitario, request.Tipo, request.UnidadeMedida);

        await ProdutoRepository.AddAsync(produto.Value, cancellationToken);

        await UnitOfWork.SaveChangesAsync(cancellationToken);

        Logger.LogInformation($"Produto {produto.Value.Nome} adicionado com sucesso.");

        return Result.Success(new Response(produto.Value.Id));
    }
}
