using Tech.Challenge.Domain.Core;
using Tech.Challenge.Domain.Exceptions;

namespace Tech.Challenge.Domain.Entities.Servico;

public class Servico : Entity
{
    public string Nome { get; private set; }

    public decimal PrecoServico { get; private set; }

    #region Foreign Keys
    
    public ICollection<ServicoOS.ServicoOS> ServicosOrdemServico { get; set; } = [];
    
    #endregion

    public static Result<Servico> Criar(string nome, decimal precoServico)
    {
        if (string.IsNullOrWhiteSpace(nome))
            return Result.Failure<Servico>(new DomainError($"Nome do serviço não pode ser vazio: {nome}"));

        if (precoServico <= 0)
            return Result.Failure<Servico>(new DomainError($"Preço serviço deve ser maior que zero: {precoServico}"));

        return Result.Success(new Servico() 
        {
            Id = Guid.NewGuid(),
            Nome = nome,
            PrecoServico = precoServico
        });
    }

    public static Result<Servico> Criar(string nome, decimal precoServico, Guid id)
    {
        if (string.IsNullOrWhiteSpace(nome))
            return Result.Failure<Servico>(new DomainError($"Nome do serviço não pode ser vazio: {nome}"));

        if (precoServico <= 0)
            return Result.Failure<Servico>(new DomainError($"Preço serviço deve ser maior que zero: {precoServico}"));

        return Result.Success(new Servico()
        {
            Id = id,
            Nome = nome,
            PrecoServico = precoServico
        });
    }

    public Result Atualizar(string nome, decimal precoServico)
    {
        if (string.IsNullOrWhiteSpace(nome))
            return Result.Failure(new DomainError($"Nome do serviço não pode ser vazio: {nome}"));
        if (precoServico <= 0)
            return Result.Failure(new DomainError($"Preço serviço deve ser maior que zero: {precoServico}"));

        Nome = nome;
        PrecoServico = precoServico;

        return Result.Success();
    }
}
