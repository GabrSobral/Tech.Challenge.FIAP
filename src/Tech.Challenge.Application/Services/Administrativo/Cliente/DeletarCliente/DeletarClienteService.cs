using Microsoft.Extensions.Logging;
using Tech.Challenge.Domain.Core;
using Tech.Challenge.Domain.Exceptions;
using Tech.Challenge.Domain.Interfaces;
using Tech.Challenge.Domain.Interfaces.Repositories;

namespace Tech.Challenge.Application.Services.Administrativo.Cliente.DeletarCliente;

public class DeletarClienteService(
    ILogger<DeletarClienteService> Logger,
    IClienteRepository ClienteRepository,
    IUnitOfWork UnitOfWork)
{
    public async Task<Result> Execute(Request request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Iniciando o processo de deleção do cliente.");

        var cliente = await ClienteRepository.GetClienteById(request.Id, cancellationToken);

        if (cliente is null)
        {
            Logger.LogWarning($"Cliente com ID {request.Id} não encontrado.");
            return Result.Failure(new ClienteNotFoundException(request.Id));
        }
        await ClienteRepository.DeleteClienteById(cliente.Id, cancellationToken);

        await UnitOfWork.SaveChangesAsync(cancellationToken);

        Logger.LogInformation($"Cliente com ID {request.Id} deletado com sucesso.");

        return Result.Success();
    }
}
