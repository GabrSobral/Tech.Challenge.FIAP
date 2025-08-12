using Microsoft.EntityFrameworkCore;
using System.Collections;
using Tech.Challenge.Domain.Entities.Cliente;
using Tech.Challenge.Domain.Entities.Cliente.ValueObjects;
using Tech.Challenge.Domain.Interfaces.Repositories;
using Tech.Challenge.Infra.Database.Contexts;

namespace Tech.Challenge.Infra.Database.Repositories;

public class ClienteRepository(DataContext dbContext) : IClienteRepository
{
    public async Task<IEnumerable<Cliente>> ListarClientes(int page, int pageSize, CancellationToken cancellationToken)
    {   
        return await dbContext.Clientes
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }
    public async Task AddCliente(Cliente cliente, CancellationToken cancellationToken)
    {
        await dbContext.AddAsync(cliente, cancellationToken);
    }

    public async Task<Cliente?> GetClienteByCpf(CPF cpf, CancellationToken cancellationToken)
    {
        return await dbContext.Clientes.FirstOrDefaultAsync(c => c.Cpf == cpf, cancellationToken);
    }

    public Task<Cliente?> GetClienteById(Guid id, CancellationToken cancellationToken)
    {
        return dbContext.Clientes.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public Task UpdateCliente(Cliente cliente, CancellationToken cancellationToken)
    {
        dbContext.Clientes.Update(cliente);

        return Task.CompletedTask;
    }

    public Task DeleteClienteById(Guid id, CancellationToken cancellationToken)
    {
        return dbContext.Clientes
            .Where(c => c.Id == id)
            .ExecuteDeleteAsync(cancellationToken);
    }
}
