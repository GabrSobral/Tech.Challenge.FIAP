using System.Collections;
using Tech.Challenge.Domain.Entities.Cliente;
using Tech.Challenge.Domain.Entities.Cliente.ValueObjects;

namespace Tech.Challenge.Domain.Interfaces.Repositories;

public interface IClienteRepository
{
    public Task<IEnumerable<Cliente>> ListarClientes(int page, int pageSize, CancellationToken cancellationToken);

    public Task<Cliente?> GetClienteByCpf(CPF cpf, CancellationToken cancellationToken);

    public Task<Cliente?> GetClienteById(Guid id, CancellationToken cancellationToken);

    public Task AddCliente(Cliente cliente, CancellationToken cancellationToken);

    public Task UpdateCliente(Cliente cliente, CancellationToken cancellationToken);

    public Task DeleteClienteById(Guid id, CancellationToken cancellationToken);
}
