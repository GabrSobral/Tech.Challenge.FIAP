using Microsoft.Extensions.Logging;
using Tech.Challenge.Domain.Core;
using Tech.Challenge.Domain.Exceptions;
using Tech.Challenge.Domain.Interfaces;
using Tech.Challenge.Domain.Interfaces.Repositories;

namespace Tech.Challenge.Application.Services.Administrativo.OrdemServico.DeletarOrdemDeServico;

public class DeletarOrdemDeServicoService(
    ILogger<DeletarOrdemDeServicoService> Logger,
    IOrdemServicoRepository OrdemServicoRepository,
    IUnitOfWork UnitOfWork)
{
    public async Task<Result> Execute(Request request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Iniciando o processo de deleção da ordem de serviço.");
        var ordemServico = await OrdemServicoRepository.GetOrdemServicoById(request.OrdemServicoId, cancellationToken);

        if (ordemServico == null)
        {
            Logger.LogWarning($"Ordem de serviço com ID {request.OrdemServicoId} não encontrada.");
            return Result.Failure(new OrdemServicoNotFoundException(request.OrdemServicoId));
        }

        ordemServico.DeletadoEm = DateTime.UtcNow;

        await OrdemServicoRepository.UpdateOrdemServico(ordemServico, cancellationToken);

        await UnitOfWork.SaveChangesAsync(cancellationToken);

        Logger.LogInformation($"Ordem de serviço com ID {request.OrdemServicoId} deletada com sucesso.");
        
        return Result.Success();
    }
}
