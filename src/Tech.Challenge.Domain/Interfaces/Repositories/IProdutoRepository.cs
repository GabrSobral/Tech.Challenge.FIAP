using Tech.Challenge.Domain.Entities.Produto;

namespace Tech.Challenge.Domain.Interfaces.Repositories;

public interface IProdutoRepository
{
    public Task<Produto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    public Task AddAsync(Produto produto, CancellationToken cancellationToken);

    public Task UpdateAsync(Produto produto, CancellationToken cancellationToken);

    public Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken);

    public Task<IEnumerable<Produto>> ListAsync(int page, int pageSize, CancellationToken cancellationToken);
}
