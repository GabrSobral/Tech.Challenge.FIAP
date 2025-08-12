using Microsoft.Extensions.Logging;
using Tech.Challenge.Domain.Core;
using Tech.Challenge.Domain.Interfaces;
using Tech.Challenge.Domain.Interfaces.Repositories;

namespace Tech.Challenge.Application.Services.Administrativo.Servicos.AdicionarServico;

public class AdicionarServicoService(
    ILogger<AdicionarServicoService> Logger,
    IServicoRepository ServicoRepository,
    IUnitOfWork UnitOfWork)
{
    public async Task<Result<Response>> Execute(Request request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Iniciando o processo de adição do serviço.");

        var servico = Tech.Challenge.Domain.Entities.Servico.Servico.Criar(request.Nome, request.PrecoServico);

        await ServicoRepository.AddAsync(servico.Value, cancellationToken);

        await UnitOfWork.SaveChangesAsync(cancellationToken);

        Logger.LogInformation($"Serviço {servico.Value.Nome} adicionado com sucesso.");

        return Result.Success(new Response(servico.Value.Id));
    }
}
