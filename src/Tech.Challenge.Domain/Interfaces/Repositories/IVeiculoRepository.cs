using Tech.Challenge.Domain.Entities.Veiculo;
using Tech.Challenge.Domain.Entities.Veiculo.ValueObjects;

namespace Tech.Challenge.Domain.Interfaces.Repositories;

public interface IVeiculoRepository
{
    public Task<Veiculo?> GetVeiculoByPlacaAsync(Placa placa, CancellationToken cancellationToken);

    public Task<Veiculo?> GetVeiculoById(Guid veiculoId, CancellationToken cancellationToken);

    public Task AddVeiculoAsync(Veiculo veiculo, CancellationToken cancellationToken);

    public Task<IEnumerable<Veiculo>> GetVeiculosByClienteIdAsync(Guid clienteId, CancellationToken cancellationToken);

    public Task DeleteVeiculoByI(Guid veiculoId, CancellationToken cancellationToken);

    public Task UpdateVeiculo(Veiculo veiculo, CancellationToken cancellationToken);
}
