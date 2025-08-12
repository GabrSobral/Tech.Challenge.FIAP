using Microsoft.Extensions.Logging;
using Tech.Challenge.Domain.Core;
using Tech.Challenge.Domain.Interfaces.Repositories;

namespace Tech.Challenge.Application.Services.Administrativo.Servicos.ListarServicos;

public class ListarServicosService(
    ILogger<ListarServicosService> Logger,
    IServicoRepository ServicoRepository)
{
    public async Task<Result<IEnumerable<Response>>> Execute(Request request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Iniciando o processo de listagem dos serviços.");
        var servicos = await ServicoRepository.ListAsync(request.Page ?? 1, request.Take ?? 50, cancellationToken);

        var servicoDtos = servicos.Select(s => new Response(s.Id, s.Nome, s.PrecoServico));

        return Result.Success(servicoDtos);
    }
}
