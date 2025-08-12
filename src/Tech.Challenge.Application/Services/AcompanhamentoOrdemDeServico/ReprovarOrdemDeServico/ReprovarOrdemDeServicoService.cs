using Microsoft.Extensions.Logging;
using Tech.Challenge.Domain.Core;
using Tech.Challenge.Domain.Exceptions;
using Tech.Challenge.Domain.Interfaces;
using Tech.Challenge.Domain.Interfaces.Repositories;

namespace Tech.Challenge.Application.Services.AcompanhamentoOrdemDeServico.ReprovarOrdemDeServico;

public class ReprovarOrdemDeServicoService(
    ILogger<ReprovarOrdemDeServicoService> Logger,
    IOrdemServicoRepository OrdemServicoRepository,
    IProdutoRepository ProdutoRepository,
    IClienteRepository ClienteRepository,
    IUnitOfWork UnitOfWork)
{
    public async Task<Result> Execute(Request request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Iniciando o processo de aprovação da ordem de serviço.");

        var cliente = await ClienteRepository.GetClienteByCpf(request.ClienteCpf, cancellationToken);

        if (cliente is null)
        {
            Logger.LogWarning($"Cliente com CPF {request.ClienteCpf} não encontrado.");
            return Result.Failure(new ClienteNotFoundException(request.ClienteCpf));
        }

        var ordemServico = await OrdemServicoRepository.GetOrdemServicoById(request.OrdemServicoId, cancellationToken);

        if (ordemServico is null)
        {
            Logger.LogWarning($"Ordem de serviço com ID {request.OrdemServicoId} não encontrada.");
            return Result.Failure(new OrdemServicoNotFoundException(request.OrdemServicoId));
        }

        if (ordemServico.ClienteCpf != cliente.Cpf)
        {
            Logger.LogWarning($"O CPF do cliente {cliente.Cpf.Valor} não corresponde ao CPF da ordem de serviço {ordemServico.ClienteCpf}.");
            return Result.Failure(new ClienteCpfMismatchException(cliente.Cpf, ordemServico.ClienteCpf));
        }

        if (ordemServico.Status != Domain.Enums.EServiceOrderStatus.AWAITING_APPROVAL)
        {
            Logger.LogInformation("Não é possível cancelar a ordem de serviço depois de aprovada... Visite a oficina mecânica presencialmente para cancelar o pedido.");
            return Result.Failure(new OrdemServicoCantBeCanceledException());
        }

        var result = ordemServico.Cancelar();

        foreach (var item in ordemServico.Produtos)
        {
            item.Produto.AdicionarEstoque(item.Quantidade);

            await ProdutoRepository.UpdateAsync(item.Produto, cancellationToken);
        }

        if (result.IsFailure)
            return Result.Failure(result.Error!);

        await OrdemServicoRepository.UpdateOrdemServico(ordemServico, cancellationToken);

        await UnitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
