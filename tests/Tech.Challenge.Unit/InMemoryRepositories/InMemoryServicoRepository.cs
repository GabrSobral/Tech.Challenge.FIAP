using Tech.Challenge.Domain.Entities.Servico;
using Tech.Challenge.Domain.Interfaces.Repositories;

namespace Tech.Challenge.Unit.InMemoryRepositories;

public class InMemoryServicoRepository : IServicoRepository
{
    public List<Servico> Dados { get; set; } = new List<Servico>();

    public Task AddAsync(Servico servico, CancellationToken cancellationToken)
    {
        Dados.Add(servico);

        return Task.CompletedTask;
    }

    public Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        Dados.RemoveAll(s => s.Id == id);

        return Task.CompletedTask;
    }

    public Task<Servico?> GetServicoById(Guid id, CancellationToken cancellationToken)
    {
        var servico = Dados.FirstOrDefault(s => s.Id == id);

        return Task.FromResult(servico);
    }

    public Task<IEnumerable<Servico>> ListAsync(int page, int pageSize, CancellationToken cancellationToken)
    {
        return Task.FromResult(Dados.Skip((page - 1) * pageSize).Take(pageSize).AsEnumerable());
    }

    public Task UpdateAsync(Servico servico, CancellationToken cancellationToken)
    {
        Dados.RemoveAll(s => s.Id == servico.Id);

        Dados.Add(servico);

        return Task.CompletedTask;
    }
}
