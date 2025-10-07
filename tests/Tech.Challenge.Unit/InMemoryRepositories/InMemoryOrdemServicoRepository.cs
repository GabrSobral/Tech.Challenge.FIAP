using Tech.Challenge.Domain.Entities.Cliente.ValueObjects;
using Tech.Challenge.Domain.Entities.OrdemServico;
using Tech.Challenge.Domain.Entities.ProdutoOS;
using Tech.Challenge.Domain.Entities.ServicoOS;
using Tech.Challenge.Domain.Interfaces.Repositories;
using Tech.Challenge.Domain.Enums;

namespace Tech.Challenge.Unit.InMemoryRepositories;

internal class InMemoryOrdemServicoRepository() : IOrdemServicoRepository
{
    public List<OrdemServico> Dados { get; set; } = new List<OrdemServico>();

    public Task AddOrdemServico(OrdemServico ordemServico, CancellationToken cancellationToken)
    {
        Dados.Add(ordemServico);

        return Task.CompletedTask;
    }

    public Task DeletarOrdemServicoProdutos(OrdemServico ordemServico, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task DeletarOrdemServicoServicos(OrdemServico ordemServico, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task DeleteOrdemServico(Guid id, CancellationToken cancellationToken)
    {
        Dados.RemoveAll(os => os.Id == id);

        return Task.CompletedTask;
    }

    public Task<OrdemServico?> GetOrdemServicoById(Guid id, CancellationToken cancellationToken)
    {
        var ordemServico = Dados.FirstOrDefault(os => os.Id == id);

        return Task.FromResult(ordemServico);
    }

    public Task<IEnumerable<OrdemServico>> GetOrdensServico(int page, int take, CancellationToken cancellationToken)
    {
        return Task.FromResult(Dados
            //.Where(x => x.DeletadoEm != null)
            .Skip((page - 1) * take)
            .Take(take)
            .AsEnumerable());
    }

    public Task<IEnumerable<OrdemServico>> GetOrdensServicoByCpf(CPF cpf, CancellationToken cancellationToken)
    {
        var ordens = Dados.Where(o => o.ClienteCpf == cpf).ToList();
        return Task.FromResult<IEnumerable<OrdemServico>>(ordens);
    }

    public Task<IEnumerable<ProdutoOS>> GetProdutosByOrdemServico(Guid ordemServicoId, CancellationToken cancellationToken)
    {
        var ordem = Dados.FirstOrDefault(o => o.Id == ordemServicoId);

        if (ordem == null)
            return Task.FromResult(Enumerable.Empty<ProdutoOS>());

        return Task.FromResult(ordem.Produtos.AsEnumerable());
    }                                                                                                                                              

    public Task<IEnumerable<ServicoOS>> GetServicosByOrdemServico(Guid ordemServicoId, CancellationToken cancellationToken)
    {
        var ordem = Dados.FirstOrDefault(o => o.Id == ordemServicoId);

        if (ordem == null)
            return Task.FromResult(Enumerable.Empty<ServicoOS>());

        // Return a copy or the original collection, depending on your needs
        return Task.FromResult(ordem.Servicos.AsEnumerable());
    }

    public Task<decimal> GetTempoMedioEntrega(CancellationToken cancellationToken)
    {
        var entregas = Dados
            .Where(o => o.Status == EServiceOrderStatus.DELIVERED && o.EntregueEm.HasValue)
            .Select(o => o.EntregueEm.Value - o.CriadaEm)
            .ToList();

        if (!entregas.Any())
            return Task.FromResult(0m);

        var averageTicks = entregas.Average(ts => ts.Ticks);
        var averageMinutes = averageTicks / (double)TimeSpan.TicksPerMinute;

        return Task.FromResult((decimal)averageMinutes);
    }


    public Task<bool> OrdemServicoExists(Guid id, CancellationToken cancellationToken)
    {
        return Task.FromResult(Dados.Any(os => os.Id == id));
    }

    public Task UpdateOrdemServico(OrdemServico ordemServico, CancellationToken cancellationToken)
    {
        var ordemServicos = Dados.Select(x => { 
            if (x.Id == ordemServico.Id)
            {
                return ordemServico;
            }

            return x;
        });

        Dados = ordemServicos.ToList();

        return Task.CompletedTask;
    }
}
