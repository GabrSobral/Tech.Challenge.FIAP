using System.ComponentModel.DataAnnotations.Schema;
using Tech.Challenge.Domain.Core;
using Tech.Challenge.Domain.Entities.Cliente.ValueObjects;
using Tech.Challenge.Domain.Entities.OrdemServico.ValueObjects;
using Tech.Challenge.Domain.Exceptions;
using Tech.Challenge.Domain.Enums;

namespace Tech.Challenge.Domain.Entities.OrdemServico;

public class OrdemServico : Entity
{
    public Guid ClienteId { get; set; }

    public CPF ClienteCpf { get; set; }

    public ICollection<ServicoOS.ServicoOS> Servicos { get; set; } = [];

    public EServiceOrderStatus Status { get; set; }

    public DateTime CriadaEm { get; set; }

    public DateTime? AtualizadaEm { get; set; }

    public DateTime? EntregueEm { get; set; }

    public Orcamento Orcamento { get; private set; }

    public ICollection<ProdutoOS.ProdutoOS> Produtos { get; set; } = [];

    [ForeignKey(nameof(ClienteId))]
    public Cliente.Cliente Cliente { get; set; }


    public static Result<OrdemServico> Criar(Guid clienteId, CPF clienteCpf, IEnumerable<ServicoOS.ServicoOS> servicos, IEnumerable<ProdutoOS.ProdutoOS> produtos, Guid? id)
    {
        var orcamento = new Orcamento();
        var orcamentoGerado = orcamento.Gerar(produtos, servicos);

        if (orcamentoGerado.IsFailure || orcamentoGerado.Value is null)
            return Result.Failure<OrdemServico>(orcamentoGerado.Error!);

        var ordemServico = new OrdemServico()
        {
            Id = id ?? Guid.NewGuid(),
            ClienteId = clienteId,
            ClienteCpf = clienteCpf,
            Status = EServiceOrderStatus.RECEIVED,
            CriadaEm = DateTime.UtcNow,
            Orcamento = orcamentoGerado.Value,
        };

        foreach (var servico in servicos)
            ordemServico.Servicos.Add(servico);

        foreach (var produto in produtos)
            ordemServico.Produtos.Add(produto);

        return Result.Success(ordemServico);
    }

    public Result Aprovar()
    {
        if (Status == EServiceOrderStatus.AWAITING_APPROVAL)
        {
            Status = EServiceOrderStatus.IN_PROGRESS;
            AtualizadaEm = DateTime.UtcNow;

            return Result.Success();
        }
        
        return Result.Failure<OrdemServico>(new DomainError("A ordem de serviço só pode ser aprovada se estiver já tiver sido recebida e diagnosticada."));
    }

    public Result AguardarAprovacao()
    {
        if (Status == EServiceOrderStatus.IN_DIAGNOSIS)
        {
            Status = EServiceOrderStatus.AWAITING_APPROVAL;
            AtualizadaEm = DateTime.UtcNow;
            return Result.Success();
        }
        
        return Result.Failure<OrdemServico>(new DomainError("A ordem de serviço só pode aguardar aprovação se estiver em diagnóstico."));
    }

    public Result Diagnosticar()
    {
        if (Status == EServiceOrderStatus.RECEIVED)
        {
            Status = EServiceOrderStatus.IN_DIAGNOSIS;
            AtualizadaEm = DateTime.UtcNow;
            return Result.Success();
        }
        
        return Result.Failure<OrdemServico>(new DomainError("A ordem de serviço só pode ser diagnosticada se estiver recebida."));
    }

    public Result Executar()
    {
        if (Status == EServiceOrderStatus.AWAITING_APPROVAL)
        {
            Status = EServiceOrderStatus.IN_PROGRESS;
            AtualizadaEm = DateTime.UtcNow;
            return Result.Success();
        }
        
        return Result.Failure<OrdemServico>(new DomainError("A ordem de serviço só pode ser executada se estiver em progresso."));
    }

    public Result Cancelar()
    {
        Status = EServiceOrderStatus.CANCELED;
        AtualizadaEm = DateTime.UtcNow;

        return Result.Success();
    }

    public Result Finalizar()
    {
        if (Status == EServiceOrderStatus.IN_PROGRESS)
        {
            Status = EServiceOrderStatus.COMPLETED;
            AtualizadaEm = DateTime.UtcNow;
            return Result.Success();
        }

        return Result.Failure<OrdemServico>(new DomainError("A ordem de serviço só pode ser finalizada se estiver completa ou entregue."));
    }

    public Result Entregar()
    {
        if (Status == EServiceOrderStatus.COMPLETED)
        {
            Status = EServiceOrderStatus.DELIVERED;
            AtualizadaEm = DateTime.UtcNow;
            EntregueEm = DateTime.UtcNow;

            return Result.Success();
        }

        return Result.Failure<OrdemServico>(new DomainError("A ordem de serviço só pode ser entregue se estiver completa."));
    }

    public Result<Orcamento> RecalcularOrcamento()
    {
        var valorOrcamento = Orcamento.Gerar(Produtos, Servicos);

        if (valorOrcamento.IsFailure)
        {
            return Result.Failure<Orcamento>(valorOrcamento.Error!);
        }

        Orcamento = valorOrcamento.Value;
        AtualizadaEm = DateTime.UtcNow;

        return Result.Success(Orcamento);
    }
}
