using Tech.Challenge.Domain.Entities.Cliente;
using Tech.Challenge.Domain.Entities.Cliente.ValueObjects;
using Tech.Challenge.Domain.Interfaces.Repositories;

namespace Tech.Challenge.Unit.InMemoryRepositories;

public class InMemoryClienteRepository : IClienteRepository
{
    public List<Cliente> Dados { get; set; } = [];

    public Task AddCliente(Cliente cliente, CancellationToken cancellationToken)
    {
        Dados.Add(cliente);

        return Task.CompletedTask;
    }

    public Task DeleteClienteById(Guid id, CancellationToken cancellationToken)
    {
        return Task.FromResult(Dados.RemoveAll(c => c.Id == id));
    }

    public Task<Cliente?> GetClienteByCpf(CPF cpf, CancellationToken cancellationToken)
    {
        var cliente = Dados.FirstOrDefault(c => c.Cpf.Valor == cpf.Valor.ToLower());

        return Task.FromResult(cliente);
    }

    public Task<Cliente?> GetClienteById(Guid id, CancellationToken cancellationToken)
    {
        return Task.FromResult(Dados.FirstOrDefault(c => c.Id == id));
    }

    public Task<IEnumerable<Cliente>> ListarClientes(int page, int pageSize, CancellationToken cancellationToken)
    {
        return Task.FromResult(Dados.Skip((page - 1) * pageSize).Take(pageSize).AsEnumerable());
    }

    public Task UpdateCliente(Cliente cliente, CancellationToken cancellationToken)
    {
        return Task.FromResult(Dados.Select(x => 
        { 
            if (x.Id == cliente.Id)
                return cliente;
            
            return x;
        }).ToList());
    }
}
