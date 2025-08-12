using Microsoft.Extensions.Logging;
using Tech.Challenge.Domain.Core;
using Tech.Challenge.Domain.Entities.Cliente.ValueObjects;
using Tech.Challenge.Domain.Exceptions;
using Tech.Challenge.Domain.Interfaces.Repositories;

namespace Tech.Challenge.Application.Services.Administrativo.Cliente.IdentificarCliente;

public class IdentificarClienteService(
    ILogger<IdentificarClienteService> Logger,
    IClienteRepository ClienteRepository,
    IVeiculoRepository VeiculoRepository)
{
    public async Task<Result<Response>> Execute(Request request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Iniciando o processo de identificação do cliente.");

        var cpfValueObject = CPF.Criar(request.Cpf);

        if (cpfValueObject.IsFailure)
            return Result.Failure<Response>(cpfValueObject.Error!);

        var cliente = await ClienteRepository.GetClienteByCpf(cpfValueObject.Value, cancellationToken);

        if (cliente is null)
            return Result.Failure<Response>(new ClienteNotFoundException());

        Logger.LogInformation($"Cliente identificado com sucesso: {cliente.Nome}");

        var clienteVeiculos = await VeiculoRepository.GetVeiculosByClienteIdAsync(cliente.Id, cancellationToken);

        return Result.Success(new Response(
            cliente.Id, 
            cliente.Cpf.Valor, 
            cliente.Nome,
            cliente.Email.Endereco,
            clienteVeiculos.Select(v => new VeiculoResponse(
                v.Id,
                v.Placa.Valor,
                v.Modelo,
                v.Ano
            )).ToList()
        ));
    }
}
