using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tech.Challenge.Domain.Core;
using Tech.Challenge.Domain.Interfaces;
using Tech.Challenge.Domain.Interfaces.Repositories;

namespace Tech.Challenge.Application.Services.Administrativo.Servicos.DeletarServico;

public class DeletarServicoService(
    ILogger<DeletarServicoService> Logger,
    IServicoRepository ServicoRepository,
    IUnitOfWork UnitOfWork)
{
    public async Task<Result> Execute(Request request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Iniciando o processo de deleção do serviço.");

        var servico = await ServicoRepository.GetServicoById(request.ServicoId, cancellationToken);

        if (servico is null)
        {
            Logger.LogWarning($"Serviço com ID {request.ServicoId} não encontrado.");
            return Result.Failure(new Exception($"Serviço com ID {request.ServicoId} não encontrado."));
        }

        await ServicoRepository.DeleteByIdAsync(servico.Id, cancellationToken);

        await UnitOfWork.SaveChangesAsync(cancellationToken);
        Logger.LogInformation($"Serviço com ID {request.ServicoId} deletado com sucesso.");
        return Result.Success();
    }
}
