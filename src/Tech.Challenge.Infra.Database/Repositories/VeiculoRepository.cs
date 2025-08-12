using Microsoft.EntityFrameworkCore;
using Tech.Challenge.Domain.Entities.Veiculo;
using Tech.Challenge.Domain.Entities.Veiculo.ValueObjects;
using Tech.Challenge.Domain.Interfaces.Repositories;
using Tech.Challenge.Infra.Database.Contexts;

namespace Tech.Challenge.Infra.Database.Repositories;

public class VeiculoRepository(DataContext dbContext) : IVeiculoRepository
{
    public async Task AddVeiculoAsync(Veiculo veiculo, CancellationToken cancellationToken)
    {
        await dbContext.Veiculos.AddAsync(veiculo, cancellationToken);
    }

    public async Task DeleteVeiculoByI(Guid veiculoId, CancellationToken cancellationToken)
    {
        await dbContext.Veiculos
            .Where(v => v.Id == veiculoId)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<Veiculo?> GetVeiculoById(Guid veiculoId, CancellationToken cancellationToken)
    {
        return await dbContext.Veiculos.FirstOrDefaultAsync(v => v.Id == veiculoId, cancellationToken);
    }

    public async Task<Veiculo?> GetVeiculoByPlacaAsync(Placa placa, CancellationToken cancellationToken)
    {
        return await dbContext.Veiculos.FirstOrDefaultAsync(v => v.Placa == placa, cancellationToken);
    }

    public async Task<IEnumerable<Veiculo>> GetVeiculosByClienteIdAsync(Guid clienteId, CancellationToken cancellationToken)
    {
        return await dbContext.Veiculos
            .Where(v => v.ClienteId == clienteId)
            .ToListAsync(cancellationToken);
    }

    public Task UpdateVeiculo(Veiculo veiculo, CancellationToken cancellationToken)
    {
        dbContext.Veiculos.Update(veiculo);

        return Task.CompletedTask;
    }
}
