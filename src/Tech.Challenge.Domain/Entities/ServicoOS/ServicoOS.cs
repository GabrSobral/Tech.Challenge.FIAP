using System.ComponentModel.DataAnnotations.Schema;
using Tech.Challenge.Domain.Core;

namespace Tech.Challenge.Domain.Entities.ServicoOS;

public class ServicoOS : Entity
{
    public Guid ServicoId { get; private set; }
    public Guid OrdemServicoId { get; private set; }

    public decimal PrecoServico { get; private set; }


    #region Foreign Keys

    [ForeignKey(nameof(ServicoId))]
    public Servico.Servico Servico { get; set; }

    [ForeignKey(nameof(OrdemServicoId))]
    public OrdemServico.OrdemServico OrdemServico { get; set; }

    #endregion

    public static ServicoOS Criar(Guid servicoId, Guid ordemServicoId, decimal precoServico)
    {
        return new ServicoOS
        {
            Id = Guid.NewGuid(),
            ServicoId = servicoId,
            OrdemServicoId = ordemServicoId,
            PrecoServico = precoServico,
        };
    }
}
