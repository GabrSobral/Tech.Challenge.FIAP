using Microsoft.Extensions.Logging;
using Tech.Challenge.Domain.Core;
using Tech.Challenge.Domain.Entities.Veiculo;
using Tech.Challenge.Domain.Entities.Veiculo.ValueObjects;
using Tech.Challenge.Domain.Exceptions;
using Tech.Challenge.Domain.Interfaces;
using Tech.Challenge.Domain.Interfaces.Repositories;

namespace Tech.Challenge.Application.Services.Administrativo.Veiculo.CadastrarVeiculo;

public class CadastrarVeiculoService(
    ILogger<CadastrarVeiculoService> Logger,
    IVeiculoRepository VeiculoRepository,
    IClienteRepository ClienteRepository,
    IUnitOfWork UnitOfWork)
{
    public async Task<Result<Response>> Execute(Request request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Iniciando o processo de cadastro do veículo.");

        var placa = Placa.Criar(request.Placa);

        if (placa.IsFailure)
            return Result.Failure<Response>(placa.Error!);

        var veiculo = await VeiculoRepository.GetVeiculoByPlacaAsync(placa.Value, cancellationToken);

        if (veiculo is not null)
            return Result.Failure<Response>(new VeiculoAlreadyRegisteredException(veiculo.Placa));

        var cliente = await ClienteRepository.GetClienteById(request.ClienteId, cancellationToken);

        if (cliente is null)
            return Result.Failure<Response>(new ClienteNotFoundException(request.ClienteId));

        var novoVeiculo = Challenge.Domain.Entities.Veiculo.Veiculo.Criar(placa.Value, request.Modelo, request.Ano, cliente.Id);

        await VeiculoRepository.AddVeiculoAsync(novoVeiculo, cancellationToken);

        await UnitOfWork.SaveChangesAsync(cancellationToken);

        Logger.LogInformation($"Veículo cadastrado com sucesso: {novoVeiculo.Placa.Valor}");

        return Result.Success(new Response(novoVeiculo.Id));
    }
}
