using Microsoft.EntityFrameworkCore;
using Tech.Challenge.Domain.Entities.Servico;
using Tech.Challenge.Domain.Interfaces.Repositories;
using Tech.Challenge.Infra.Database.Contexts;

namespace Tech.Challenge.Infra.Database.Repositories;

public class ServicoRepository(DataContext dbContext) : IServicoRepository
{
    public async Task AddAsync(Servico servico, CancellationToken cancellationToken)
    {
        await dbContext.Servicos.AddAsync(servico, cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        await dbContext.Servicos
            .Where(s => s.Id == id)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        await dbContext.Servicos
            .Where(s => s.Id == id)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<Servico?> GetServicoById(Guid id, CancellationToken cancellationToken)
    {
        return await dbContext.Servicos
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public Task<IEnumerable<Servico>> ListAsync(int page, int pageSize, CancellationToken cancellationToken)
    {
        return dbContext.Servicos
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken)
            .ContinueWith(task => task.Result.AsEnumerable(), cancellationToken);
    }

    public Task UpdateAsync(Servico servico, CancellationToken cancellationToken)
    {
        dbContext.Servicos.Update(servico);

        return Task.CompletedTask;
    }
}
