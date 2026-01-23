using Microsoft.Extensions.Logging;
using NewRelic.Api.Agent;
using Tech.Challenge.Domain.Core;
using Tech.Challenge.Domain.Exceptions;
using Tech.Challenge.Domain.Interfaces;
using Tech.Challenge.Domain.Interfaces.Repositories;

namespace Tech.Challenge.Application.Services.Administrativo.OrdemServico.AtualizarStatusOrdemDeServico;

public class AtualizarStatusOrdemDeServico(
    ILogger<AtualizarStatusOrdemDeServico> Logger,
    IOrdemServicoRepository OrdemServicoRepository,
    IClienteRepository ClienteRepository,
    // IMailService MailService,
    IUnitOfWork UnitOfWork)
{
    [Transaction]
    [Trace]
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

        var cliente = await ClienteRepository.GetClienteById(ordemServico.ClienteId, cancellationToken);

        if (cliente is null)
            return Result.Failure(new ClienteNotFoundException(ordemServico.ClienteId));

        // await MailService.SendOrdemServicoStatusMail(cliente.Email, ordemServico.Status, cancellationToken);

        var duracaoNoStatusAnterior = (DateTime.UtcNow - (ordemServico.AtualizadaEm ?? DateTime.UtcNow)).TotalSeconds;
        var eventAttributes = new Dictionary<string, object>
        {
            { "os_id", ordemServico.Id },
            { "status_anterior", ordemServico.Status.ToString() }, // Ex: IN_DIAGNOSIS
            { "novo_status", request.Status.ToString() },    // Ex: AWAITING_APPROVAL
            { "duracao_segundos", duracaoNoStatusAnterior },
            { "valor_orcamento", ordemServico.Orcamento?.Valor ?? 0 }
        };

        NewRelic.Api.Agent.NewRelic.RecordCustomEvent("MovimentacaoOS", eventAttributes);

        return Result.Success();
    }
}
