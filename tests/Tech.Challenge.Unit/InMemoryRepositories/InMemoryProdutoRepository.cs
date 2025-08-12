using Tech.Challenge.Domain.Entities.Produto;
using Tech.Challenge.Domain.Entities.Servico;
using Tech.Challenge.Domain.Interfaces.Repositories;

namespace Tech.Challenge.Unit.InMemoryRepositories;

public class InMemoryProdutoRepository : IProdutoRepository
{
    public List<Produto> Dados { get; set; } = new List<Produto>();

    public Task AddAsync(Produto produto, CancellationToken cancellationToken)
    {
        Dados.Add(produto);

        return Task.CompletedTask;
    }

    public Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        Dados.RemoveAll(p => p.Id == id);
        
        return Task.CompletedTask;
    }

    public Task<Produto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return Task.FromResult(Dados.FirstOrDefault(p => p.Id == id));
    }

    public Task<IEnumerable<Produto>> ListAsync(int page, int pageSize, CancellationToken cancellationToken)
    {
        return Task.FromResult(Dados.Skip((page - 1) * pageSize).Take(pageSize).AsEnumerable());
    }

    public Task UpdateAsync(Produto produto, CancellationToken cancellationToken)
    {
        var index = Dados.FindIndex(p => p.Id == produto.Id);

        if (index != -1)
        {
            Dados[index] = produto;
        }

        return Task.CompletedTask;
    }
}
