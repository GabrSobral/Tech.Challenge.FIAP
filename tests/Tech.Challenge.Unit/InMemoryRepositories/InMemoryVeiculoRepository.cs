using Tech.Challenge.Domain.Entities.Cliente.ValueObjects;
using Tech.Challenge.Domain.Entities.Veiculo;
using Tech.Challenge.Domain.Entities.Veiculo.ValueObjects;
using Tech.Challenge.Domain.Interfaces.Repositories;

namespace Tech.Challenge.Unit.InMemoryRepositories;

public class InMemoryVeiculoRepository : IVeiculoRepository
{
    public List<Veiculo> Dados { get; } = [];

    public Task AddVeiculoAsync(Veiculo veiculo, CancellationToken cancellationToken)
    {
        Dados.Add(veiculo);

        return Task.CompletedTask;
    }

    public Task DeleteVeiculoByI(Guid veiculoId, CancellationToken cancellationToken)
    {
        Dados.RemoveAll(v => v.Id == veiculoId);

        return Task.CompletedTask;
    }

    public Task<Veiculo?> GetVeiculoById(Guid veiculoId, CancellationToken cancellationToken)
    {
        return Task.FromResult(Dados.FirstOrDefault(v => v.Id == veiculoId));
    }

    public Task<Veiculo?> GetVeiculoByPlacaAsync(Placa placa, CancellationToken cancellationToken)
    {
        var cliente = Dados.FirstOrDefault(c => c.Placa == placa);

        return Task.FromResult(cliente);
    }

    public Task<IEnumerable<Veiculo>> GetVeiculosByClienteIdAsync(Guid clienteId, CancellationToken cancellationToken)
    {
        return Task.FromResult(Dados.Where(v => v.ClienteId == clienteId).AsEnumerable());
    }

    public Task UpdateVeiculo(Veiculo veiculo, CancellationToken cancellationToken)
    {
        var index = Dados.FindIndex(v => v.Id == veiculo.Id);

        if (index != -1)
        {
            Dados[index] = veiculo;
        }

        return Task.CompletedTask;
    }
}
