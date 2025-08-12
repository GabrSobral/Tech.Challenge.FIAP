using Microsoft.Extensions.Logging;
using Tech.Challenge.Domain.Core;
using Tech.Challenge.Domain.Entities.Veiculo.ValueObjects;
using Tech.Challenge.Domain.Exceptions;
using Tech.Challenge.Domain.Interfaces.Repositories;

namespace Tech.Challenge.Application.Services.Administrativo.Veiculo.ConsultarVeiculoPelaPlaca;

public class ConsultarVeiculoPelaPlacaService(
        ILogger<ConsultarVeiculoPelaPlacaService> Logger,
        IVeiculoRepository VeiculoRepository)
{
    public async Task<Result<Response>> Execute(Request request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Iniciando o processo de consulta do veículo pela placa.");

        var placa = Placa.Criar(request.Placa);

        if (placa.IsFailure)
            return Result.Failure<Response>(placa.Error!);

        var veiculo = await VeiculoRepository.GetVeiculoByPlacaAsync(placa.Value, cancellationToken);

        if (veiculo is null)
            return Result.Failure<Response>(new VeiculoNotFoundException(placa.Value));

        Logger.LogInformation($"Veículo encontrado: {veiculo.Placa.Valor}");

        return Result.Success(new Response(
            veiculo.Id,
            veiculo.Placa.Valor,
            veiculo.Modelo,
            veiculo.Ano));
    }
}
