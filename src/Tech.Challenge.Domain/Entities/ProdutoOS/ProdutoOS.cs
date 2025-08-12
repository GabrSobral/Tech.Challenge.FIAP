using System.ComponentModel.DataAnnotations.Schema;
using Tech.Challenge.Domain.Core;

namespace Tech.Challenge.Domain.Entities.ProdutoOS;

// Tabela associativa entre Produto e Ordem de Serviço para o EF Core
public class ProdutoOS : Entity
{
    public uint Quantidade { get; private set; }

    public decimal PrecoUnitario { get; private set; }

    public Guid ProdutoId { get; private set; }

    public Guid OrdemServicoId { get; private set; }

    #region Foreign Keys

    [ForeignKey(nameof(ProdutoId))]
    public Produto.Produto Produto { get; set; }

    [ForeignKey(nameof(OrdemServicoId))]
    public OrdemServico.OrdemServico OrdemServico { get; set; }

    #endregion

    public static ProdutoOS Criar(Guid produtoId, Guid ordemServicoId, uint quantidade, decimal precoUnitario)
    {
        return new ProdutoOS()
        {
            Id = Guid.NewGuid(),
            ProdutoId = produtoId,
            OrdemServicoId = ordemServicoId,
            Quantidade = quantidade,
            PrecoUnitario = precoUnitario
        };
    }
}
