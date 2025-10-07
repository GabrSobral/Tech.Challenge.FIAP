using Microsoft.EntityFrameworkCore;
using Tech.Challenge.Domain.Entities.Cliente.ValueObjects;
using Tech.Challenge.Domain.Entities.OrdemServico;
using Tech.Challenge.Domain.Entities.ProdutoOS;
using Tech.Challenge.Domain.Entities.ServicoOS;
using Tech.Challenge.Domain.Interfaces.Repositories;
using Tech.Challenge.Domain.Enums;
using Tech.Challenge.Infra.Database.Contexts;

namespace Tech.Challenge.Infra.Database.Repositories;

public class OrdemServicoRepository(DataContext dbContext) : IOrdemServicoRepository
{
    public async Task AddOrdemServico(OrdemServico ordemServico, CancellationToken cancellationToken)
    {
        await dbContext.OrdemServicos.AddAsync(ordemServico, cancellationToken);
    }

    public async Task DeleteOrdemServico(Guid id, CancellationToken cancellationToken)
    {
        await dbContext.OrdemServicos
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<OrdemServico?> GetOrdemServicoById(Guid id, CancellationToken cancellationToken)
    {
        return await dbContext.OrdemServicos
            .Include(x => x.Produtos)
                .ThenInclude(x => x.Produto)
            .Include(x => x.Servicos)
                .ThenInclude(x => x.Servico)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<ServicoOS>> GetServicosByOrdemServico(Guid ordemServicoId, CancellationToken cancellationToken)
    {
        return await dbContext.ServicosOrdemServico.Where(x => x.OrdemServicoId == ordemServicoId)
            .Include(s => s.Servico)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ProdutoOS>> GetProdutosByOrdemServico(Guid ordemServicoId, CancellationToken cancellationToken)
    {
        return await dbContext.ProdutosOrdemServico.Where(x => x.OrdemServicoId == ordemServicoId)
            .Include(s => s.Produto)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<OrdemServico>> GetOrdensServicoByCpf(CPF cpf, CancellationToken cancellationToken)
    {
        return await dbContext.OrdemServicos
            .Where(x => x.ClienteCpf == cpf)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> OrdemServicoExists(Guid id, CancellationToken cancellationToken)
    {
        return await dbContext.OrdemServicos.AnyAsync(x => x.Id == id, cancellationToken);
    }

    public Task UpdateOrdemServico(OrdemServico ordemServico, CancellationToken cancellationToken)
    {
        dbContext.OrdemServicos.Update(ordemServico);

        return Task.CompletedTask;
    }

    public async Task DeletarOrdemServicoProdutos(OrdemServico ordemServico, CancellationToken cancellationToken)
    {
        await dbContext.ProdutosOrdemServico
            .Where(x => x.OrdemServicoId == ordemServico.Id)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task DeletarOrdemServicoServicos(OrdemServico ordemServico, CancellationToken cancellationToken)
    {
        await dbContext.ServicosOrdemServico
            .Where(x => x.OrdemServicoId == ordemServico.Id)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<decimal> GetTempoMedioEntrega(CancellationToken cancellationToken)
    {
        var timeSpans = await dbContext.OrdemServicos
            .Where(o => o.Status == EServiceOrderStatus.DELIVERED && o.EntregueEm.HasValue)
            .Select(o => o.EntregueEm!.Value - o.CriadaEm)
            .ToListAsync(cancellationToken);

        if (timeSpans.Count == 0)
            return 0m;

        var averageTicks = timeSpans.Average(ts => ts.Ticks);
        var averageMinutes = averageTicks / (double)TimeSpan.TicksPerMinute;

        return (decimal)averageMinutes;
    }

    public async Task<IEnumerable<OrdemServico>> GetOrdensServico(int page, int take, CancellationToken cancellationToken)
    {
        return await dbContext.OrdemServicos
            .Skip((page - 1) * take)
            .Take(take)
            .Where(x => x.DeletadoEm != null)
            .ToListAsync(cancellationToken);
    }
}
