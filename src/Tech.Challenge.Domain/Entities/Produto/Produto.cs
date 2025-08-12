using Tech.Challenge.Domain.Core;
using Tech.Challenge.Domain.Enums;
using Tech.Challenge.Domain.Exceptions;

namespace Tech.Challenge.Domain.Entities.Produto;

public class Produto : Entity
{
    public string Nome { get; private set; }

    public uint Quantidade { get; private set; }
    
    public decimal PrecoUnitario { get; private set; }

    public ETipoProduto Tipo { get; private set; } = ETipoProduto.INSUMO;

    public EUnidadeMedida UnidadeMedida { get; private set; } = EUnidadeMedida.UNIDADE;

    #region Foreign Keys

    public ICollection<ProdutoOS.ProdutoOS> ProdutosOrdemServico { get; set; } = [];

    #endregion


    public static Result<Produto> Criar(string nome, uint quantidade, decimal precoUnitarioBruto, ETipoProduto tipo, EUnidadeMedida unidadeMedida)
    {
        if (string.IsNullOrWhiteSpace(nome))
            return Result.Failure<Produto>(new DomainError($"Nome da peça não pode ser vazio: {nome}"));

        if (quantidade <= 0)
            return Result.Failure<Produto>(new DomainError($"Quantidade deve ser maior que zero: {quantidade}"));

        if (precoUnitarioBruto <= 0)
            return Result.Failure<Produto>(new DomainError($"Preço unitário deve ser maior que zero? {precoUnitarioBruto}"));

        return Result.Success(new Produto 
        {
            Id = Guid.NewGuid(),
            Nome = nome,
            Quantidade = quantidade,
            PrecoUnitario = precoUnitarioBruto,
            Tipo = tipo,
            UnidadeMedida = unidadeMedida
        });
    }

    public static Result<Produto> Criar(string nome, uint quantidade, decimal precoUnitarioBruto, ETipoProduto tipo, EUnidadeMedida unidadeMedida, Guid id)
    {
        if (string.IsNullOrWhiteSpace(nome))
            return Result.Failure<Produto>(new DomainError($"Nome da peça não pode ser vazio: {nome}"));

        if (quantidade <= 0)
            return Result.Failure<Produto>(new DomainError($"Quantidade deve ser maior que zero: {quantidade}"));

        if (precoUnitarioBruto <= 0)
            return Result.Failure<Produto>(new DomainError($"Preço unitário deve ser maior que zero? {precoUnitarioBruto}"));

        return Result.Success(new Produto
        {
            Id = id,
            Nome = nome,
            Quantidade = quantidade,
            PrecoUnitario = precoUnitarioBruto,
            Tipo = tipo,
            UnidadeMedida = unidadeMedida
        });
    }

    public Result Atualizar(string nome, uint quantidade, decimal precoUnitarioBruto, ETipoProduto tipo, EUnidadeMedida unidadeMedida)
    {
        if (string.IsNullOrWhiteSpace(nome))
            return Result.Failure(new DomainError($"Nome da peça não pode ser vazio: {nome}"));

        if (quantidade < 0)
            return Result.Failure(new DomainError($"Quantidade deve ser maior ou igual a zero: {quantidade}"));


        if (precoUnitarioBruto < 0)
            return Result.Failure(new DomainError($"Preço unitário deve ser maior ou igual a zero? {precoUnitarioBruto}"));

        Quantidade = quantidade;
        Nome = nome;
        PrecoUnitario = precoUnitarioBruto;
        UnidadeMedida = unidadeMedida;

        return Result.Success();
    }

    public Result DiminuirEstoque(uint quantidade)
    {
        if (quantidade <= 0)
            return Result.Failure(new DomainError($"Quantidade deve ser maior que zero: {quantidade}"));

        if (Quantidade < quantidade)
            return Result.Failure(new DomainError($"Estoque insuficiente. Quantidade atual: {Quantidade}, Quantidade solicitada: {quantidade}"));

        Quantidade -= quantidade;

        return Result.Success();
    }

    public Result AdicionarEstoque(uint quantidade)
    {
        if (quantidade <= 0)
            return Result.Failure(new DomainError($"Quantidade deve ser maior que zero: {quantidade}"));

        Quantidade += quantidade;

        return Result.Success();
    }
}
