using Tech.Challenge.Domain.Core;
using Tech.Challenge.Domain.Entities.Cliente.ValueObjects;

namespace Tech.Challenge.Domain.Entities.Cliente;

public class Cliente : Entity
{
    public required CPF Cpf { get; set; }
    public required string Nome { get; set; }
    public required Email Email { get; set; }
    public required DateTime AdicionadoEm { get; set; }
    public DateTime? AtuazadoEm { get; set; }

    // Para o EF Core fazer a relação
    public ICollection<OrdemServico.OrdemServico> OrdemServicos { get; set; } = [];
    public ICollection<Veiculo.Veiculo> Veiculos { get; set; } = [];

    public static Cliente Criar(CPF cpf, string nome, Email email)
    {
        return new Cliente()
        {
            Id = Guid.NewGuid(),
            Cpf = cpf,
            Nome = nome,
            Email = email,
            AdicionadoEm = DateTime.UtcNow,
        };
    }

    public static Cliente Criar(CPF cpf, string nome, Email email, Guid id)
    {
        return new Cliente()
        {
            Id = id,
            Cpf = cpf,
            Nome = nome,
            Email = email,
            AdicionadoEm = DateTime.UtcNow,
        };
    }


    public void Atualizar(CPF cpf, string nome, Email email)
    {
        Cpf = cpf;
        Nome = nome;
        Email = email;
        AtuazadoEm = DateTime.UtcNow;
    }
}
