using Tech.Challenge.Domain.Entities.Cliente.ValueObjects;
using Tech.Challenge.Domain.Entities.OrdemServico;
using Tech.Challenge.Domain.Entities.ProdutoOS;
using Tech.Challenge.Domain.Entities.ServicoOS;

namespace Tech.Challenge.Domain.Interfaces.Repositories;

public  interface IOrdemServicoRepository
{
    Task<decimal> GetTempoMedioEntrega(CancellationToken cancellationToken);

    Task<IEnumerable<OrdemServico>> GetOrdensServico(int page, int take, CancellationToken cancellationToken);

    Task<OrdemServico?> GetOrdemServicoById(Guid id, CancellationToken cancellationToken);

    Task<IEnumerable<OrdemServico>> GetOrdensServicoByCpf(CPF cpf, CancellationToken cancellationToken);

    Task<IEnumerable<ServicoOS>> GetServicosByOrdemServico(Guid ordemServicoId, CancellationToken cancellationToken);

    Task<IEnumerable<ProdutoOS>> GetProdutosByOrdemServico(Guid ordemServicoId, CancellationToken cancellationToken);

    Task<bool> OrdemServicoExists(Guid id, CancellationToken cancellationToken);

    Task AddOrdemServico(OrdemServico ordemServico, CancellationToken cancellationToken);

    Task UpdateOrdemServico(OrdemServico ordemServico, CancellationToken cancellationToken);

    Task DeleteOrdemServico(Guid id, CancellationToken cancellationToken);

    Task DeletarOrdemServicoProdutos(OrdemServico ordemServico, CancellationToken cancellationToken);

    Task DeletarOrdemServicoServicos(OrdemServico ordemServico, CancellationToken cancellationToken);
}
