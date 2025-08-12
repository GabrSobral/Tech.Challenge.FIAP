using Microsoft.Extensions.Logging;
using Tech.Challenge.Domain.Core;
using Tech.Challenge.Domain.Exceptions;
using Tech.Challenge.Domain.Interfaces;
using Tech.Challenge.Domain.Interfaces.Repositories;
using Tech.Challenge.Application.Services.AcompanhamentoOrdemDeServico.AprovarOrdemDeServico;

namespace Tech.Challenge.Application.Services.Administrativo.OrdemServico.AtualizarStatusOrdemDeServico;

public class AtualizarStatusOrdemDeServico(
      ILogger<AtualizarStatusOrdemDeServico> Logger,
      IOrdemServicoRepository OrdemServicoRepository,
      IUnitOfWork UnitOfWork)
{
    public async Task<Result> Execute(Request request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Iniciando o processo de aprovação da ordem de serviço.");

        var ordemServico = await OrdemServicoRepository.GetOrdemServicoById(request.OrdemServicoId, cancellationToken);

        if (ordemServico is null)
        {
            Logger.LogWarning($"Ordem de serviço com ID {request.OrdemServicoId} não encontrada.");
            return Result.Failure(new OrdemServicoNotFoundException(request.OrdemServicoId));
        }

        Result result = null;

        if (request.Status == Domain.Enums.EServiceOrderStatus.IN_DIAGNOSIS)
            result = ordemServico.Diagnosticar();

        if (request.Status == Domain.Enums.EServiceOrderStatus.AWAITING_APPROVAL)
            result = ordemServico.AguardarAprovacao();

        if (request.Status == Domain.Enums.EServiceOrderStatus.IN_PROGRESS)
            result = ordemServico.Executar();

        if (request.Status == Domain.Enums.EServiceOrderStatus.COMPLETED)
            result = ordemServico.Finalizar();

        if (request.Status == Domain.Enums.EServiceOrderStatus.DELIVERED)
            result = ordemServico.Entregar();

        if (request.Status == Domain.Enums.EServiceOrderStatus.CANCELED)
            result = ordemServico.Cancelar();

        if (result is not null && result.IsFailure)
            return Result.Failure(result.Error!);

        await OrdemServicoRepository.UpdateOrdemServico(ordemServico, cancellationToken);

        await UnitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
