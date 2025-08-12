using Microsoft.Extensions.Logging;
using Tech.Challenge.Domain.Core;
using Tech.Challenge.Domain.Exceptions;
using Tech.Challenge.Domain.Interfaces;
using Tech.Challenge.Domain.Interfaces.Repositories;

namespace Tech.Challenge.Application.Services.Administrativo.Cliente.AtualizarCliente;

public class AtualizarClienteService(
    ILogger<AtualizarClienteService> Logger,
    IClienteRepository ClienteRepository,
    IUnitOfWork UnitOfWork)
{
    public async Task<Result> Execute(Request request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Iniciando o processo de atualização do cliente.");

        var cliente = await ClienteRepository.GetClienteById(request.Id, cancellationToken);

        if (cliente is null)
        {
            Logger.LogWarning($"Cliente com ID {request.Id} não encontrado.");
            return Result.Failure(new ClienteNotFoundException(request.Id));
        }

        cliente.Atualizar(request.Cpf, request.Nome, request.Email);

        await ClienteRepository.UpdateCliente(cliente, cancellationToken);

        await UnitOfWork.SaveChangesAsync(cancellationToken);

        Logger.LogInformation($"Cliente com ID {request.Id} atualizado com sucesso.");

        return Result.Success();
    }
}
