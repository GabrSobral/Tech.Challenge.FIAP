using Microsoft.Extensions.Logging;
using Tech.Challenge.Domain.Core;
using Tech.Challenge.Domain.Exceptions;
using Tech.Challenge.Domain.Interfaces;
using Tech.Challenge.Domain.Interfaces.Repositories;

namespace Tech.Challenge.Application.Services.Administrativo.Veiculo.AtualizarVeiculo;

public class AtualizarVeiculoService(
    ILogger<AtualizarVeiculoService> Logger,
    IVeiculoRepository VeiculoRepository,
    IUnitOfWork UnitOfWork)
{
    public async Task<Result> Execute(Request request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Iniciando o processo de atualização do veículo.");
        var veiculo = await VeiculoRepository.GetVeiculoById(request.VeiculoId, cancellationToken);

        if (veiculo is null)
        {
            Logger.LogWarning($"Veículo com ID {request.VeiculoId} não encontrado.");
            return Result.Failure(new VeiculoNotFoundException(request.VeiculoId));
        }

        veiculo.Atualizar(request.Placa, request.Modelo, request.Ano);

        await VeiculoRepository.UpdateVeiculo(veiculo, cancellationToken);

        await UnitOfWork.SaveChangesAsync(cancellationToken);

        Logger.LogInformation($"Veículo com ID {request.VeiculoId} atualizado com sucesso.");

        return Result.Success();
    }
}
