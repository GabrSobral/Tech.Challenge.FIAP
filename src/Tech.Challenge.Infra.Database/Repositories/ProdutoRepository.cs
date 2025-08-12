using Microsoft.EntityFrameworkCore;
using Tech.Challenge.Domain.Entities.Produto;
using Tech.Challenge.Domain.Interfaces.Repositories;
using Tech.Challenge.Infra.Database.Contexts;

namespace Tech.Challenge.Infra.Database.Repositories;

public class ProdutoRepository(DataContext dbContext) : IProdutoRepository
{
    public async Task AddAsync(Produto produto, CancellationToken cancellationToken)
    {
        await dbContext.Produtos.AddAsync(produto, cancellationToken);
    }

    public async Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        await dbContext.Produtos
            .Where(p => p.Id == id)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public Task<Produto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return dbContext.Produtos
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Produto>> ListAsync(int page, int pageSize, CancellationToken cancellationToken)
    {
        return await dbContext.Produtos
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .OrderBy(p => p.Nome)
            .ToListAsync(cancellationToken);
    }

    public Task UpdateAsync(Produto produto, CancellationToken cancellationToken)
    {
        dbContext.Produtos.Update(produto);

        return Task.CompletedTask;
    }
}
