using Microsoft.Extensions.Logging;
using Tech.Challenge.Domain.Core;
using Tech.Challenge.Domain.Entities.OrdemServico;
using Tech.Challenge.Domain.Entities.ProdutoOS;
using Tech.Challenge.Domain.Entities.ServicoOS;
using Tech.Challenge.Domain.Exceptions;
using Tech.Challenge.Domain.Interfaces;
using Tech.Challenge.Domain.Interfaces.Repositories;
using Tech.Challenge.Infra.Database.Utils;

namespace Tech.Challenge.Application.Services.Administrativo.OrdemServico.CriarOrdemDeServico;

public class CriarOrdemDeServicoService(
    ILogger<CriarOrdemDeServicoService> Logger,
    IServicoRepository servicoRepository,
    IOrdemServicoRepository ordemServicoRepository,
    IProdutoRepository produtoRepository,
    IClienteRepository clienteRepository,
    IUnitOfWork UnitOfWork)
{
    public async Task<Result<Response>> Execute(Request request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Iniciando o processo de criação da ordem de serviço.");

        List<ServicoOS> servicos = [];
        List<ProdutoOS> produtos = [];

        var cliente = await clienteRepository.GetClienteById(request.ClienteId, cancellationToken);

        if (cliente is null)
            return Result.Failure<Response>(new ClienteNotFoundException(request.ClienteId));

        var ordemServicoId = Guid.NewGuid();

        try
        {
            foreach (var x in request.Servicos)
            {
                var servico = await servicoRepository.GetServicoById(x, cancellationToken)
                    ?? throw new ServicoNotFoundException(x);

                servicos.Add(ServicoOS.Criar(servico.Id, ordemServicoId, servico.PrecoServico));
            }

            foreach (var x in request.Produtos)
            {
                var produto = await produtoRepository.GetByIdAsync(x.Id, cancellationToken) 
                    ?? throw new ProductNotFoundException(x.Id);

                produtos.Add(ProdutoOS.Criar(produto.Id, ordemServicoId, x.Quantidade, produto.PrecoUnitario));

                var result = produto.DiminuirEstoque(x.Quantidade);

                if (result.IsFailure)
                {
                    Logger.LogError(result.Error, "Erro ao diminuir estoque do produto.");
                    throw result.Error!;
                }

                await produtoRepository.UpdateAsync(produto, cancellationToken);
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Erro ao buscar serviços ou produtos.");
            return Result.Failure<Response>(ex);
        }

        var ordemServicoResult = Challenge.Domain.Entities.OrdemServico.OrdemServico.Criar(request.ClienteId, cliente.Cpf, servicos, produtos, ordemServicoId);

        if (ordemServicoResult.IsFailure)
        {
            Logger.LogError(ordemServicoResult.Error, "Erro ao criar ordem de serviço.");
            return Result.Failure<Response>(ordemServicoResult.Error!);
        }

        var ordemServico = ordemServicoResult.Value;

        if (ordemServico is not null)
            await ordemServicoRepository.AddOrdemServico(ordemServico, cancellationToken);

        var response = new Response(ordemServico!.Id);

        await UnitOfWork.SaveChangesAsync(cancellationToken);

        Logger.LogInformation($"Ordem de serviço criada com ID: {ordemServico.Id}");

        return Result.Success(response);
    }
}
