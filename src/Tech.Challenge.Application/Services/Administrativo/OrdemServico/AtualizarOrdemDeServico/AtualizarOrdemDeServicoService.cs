using Microsoft.Extensions.Logging;
using Tech.Challenge.Domain.Core;
using Tech.Challenge.Domain.Entities.ProdutoOS;
using Tech.Challenge.Domain.Entities.ServicoOS;
using Tech.Challenge.Domain.Exceptions;
using Tech.Challenge.Domain.Interfaces;
using Tech.Challenge.Domain.Interfaces.Repositories;

namespace Tech.Challenge.Application.Services.Administrativo.OrdemServico.AtualizarOrdemDeServico;

public class AtualizarOrdemDeServicoService(
    ILogger<AtualizarOrdemDeServicoService> Logger,
    IOrdemServicoRepository OrdemServicoRepository,
    IServicoRepository ServicoRepository,
    IProdutoRepository ProdutoRepository,
    IUnitOfWork UnitOfWork)
{
    public async Task<Result> Execute(Request request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Iniciando o processo de criação da ordem de serviço.");

        var ordemServico = await OrdemServicoRepository.GetOrdemServicoById(request.OrdemServicoId, cancellationToken);

        if (ordemServico == null)
        {
            Logger.LogWarning($"Ordem de serviço com ID {request.OrdemServicoId} não encontrada.");
            return Result.Failure(new OrdemServicoNotFoundException(request.OrdemServicoId));
        }

        List<ServicoOS> servicos = [];
        List<ProdutoOS> produtos = [];

        //await OrdemServicoRepository.DeletarOrdemServicoProdutos(ordemServico, cancellationToken);
        //await OrdemServicoRepository.DeletarOrdemServicoServicos(ordemServico, cancellationToken);

        try
        {
            foreach (var x in request.Servicos)
            {
                var servico = await ServicoRepository.GetServicoById(x, cancellationToken)
                    ?? throw new ServicoNotFoundException(x);

                servicos.Add(ServicoOS.Criar(servico.Id, ordemServico.Id, servico.PrecoServico));
            }

            foreach (var x in request.Produtos)
            {
                var produto = await ProdutoRepository.GetByIdAsync(x.Id, cancellationToken)
                    ?? throw new ProductNotFoundException(x.Id);

                produtos.Add(ProdutoOS.Criar(produto.Id, ordemServico.Id, x.Quantidade, produto.PrecoUnitario));
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Erro ao buscar serviços ou produtos.");
            return Result.Failure(ex);
        }

        ordemServico.Produtos = produtos;
        ordemServico.Servicos = servicos;

        ordemServico.RecalcularOrcamento();

        await OrdemServicoRepository.UpdateOrdemServico(ordemServico, cancellationToken);

        await UnitOfWork.SaveChangesAsync(cancellationToken);

        Logger.LogInformation($"Ordem de serviço atualizada com ID: {ordemServico.Id}");

        return Result.Success();
    }
}
