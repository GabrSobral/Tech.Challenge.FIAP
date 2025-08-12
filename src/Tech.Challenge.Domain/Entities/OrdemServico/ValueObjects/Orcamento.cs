using Tech.Challenge.Domain.Core;

namespace Tech.Challenge.Domain.Entities.OrdemServico.ValueObjects;

public class Orcamento : ValueObject<Orcamento>
{
    public decimal Valor { get; } = 0;
    public DateTime? ReajustadoEm { get; private set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Valor;
    }

    public Orcamento(decimal valor)
    {
        Valor = valor;
    }

    public Orcamento() { }

    public Result<Orcamento> Gerar(IEnumerable<ProdutoOS.ProdutoOS> produtos, IEnumerable<ServicoOS.ServicoOS> servicos)
    {
        var precoTotal = produtos.Sum(p => p.PrecoUnitario * p.Quantidade) + servicos.Sum(s => s.PrecoServico);

        return Result.Success(new Orcamento(precoTotal));
    }
}
