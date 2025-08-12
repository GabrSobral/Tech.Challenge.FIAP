using Tech.Challenge.Domain.Entities.Servico;

namespace Tech.Challenge.Domain.Interfaces.Repositories;

public interface IServicoRepository
{
    public Task<Servico?> GetServicoById(Guid id, CancellationToken cancellationToken);

    public Task AddAsync(Servico servico, CancellationToken cancellationToken);

    public Task UpdateAsync(Servico servico, CancellationToken cancellationToken);

    public Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken);

    public Task<IEnumerable<Servico>> ListAsync(int page, int pageSize, CancellationToken cancellationToken);
}
