using Microsoft.Extensions.Logging;
using Tech.Challenge.Domain.Core;
using Tech.Challenge.Domain.Interfaces;
using Tech.Challenge.Domain.Interfaces.Repositories;

namespace Tech.Challenge.Application.Services.Administrativo.Servicos.AtualizarServico;

public class AtualizarServicoService(
    ILogger<AtualizarServicoService> Logger,
    IServicoRepository ServicoRepository,
    IUnitOfWork UnitOfWork)
{
    public async Task<Result> Execute(Request request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Iniciando o processo de atualização do serviço.");

        var servico = await ServicoRepository.GetServicoById(request.Id, cancellationToken);

        if (servico is null)
        {
            Logger.LogWarning($"Serviço com ID {request.Id} não encontrado.");
            return Result.Failure(new Exception($"Serviço com ID {request.Id} não encontrado."));
        }

        var result = servico.Atualizar(request.Nome, request.PrecoServico);

        if (result.IsFailure)
            return Result.Failure(result.Error!);

        await ServicoRepository.UpdateAsync(servico, cancellationToken);

        await UnitOfWork.SaveChangesAsync(cancellationToken);

        Logger.LogInformation($"Serviço com ID {request.Id} atualizado com sucesso.");

        return Result.Success();
    }
}
