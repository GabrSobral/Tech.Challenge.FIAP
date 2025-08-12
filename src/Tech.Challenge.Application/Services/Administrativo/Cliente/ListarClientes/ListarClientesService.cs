using Microsoft.Extensions.Logging;
using Tech.Challenge.Domain.Core;
using Tech.Challenge.Domain.Interfaces.Repositories;

namespace Tech.Challenge.Application.Services.Administrativo.Cliente.ListarClientes;

public class ListarClientesService(
    ILogger<ListarClientesService> Logger,
    IClienteRepository ClienteRepository)
{
    public async Task<Result<IEnumerable<Response>>> Handle(Request request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Iniciando o processo de listagem de clientes.");

        var clientes = await ClienteRepository.ListarClientes(request.Page ?? 1, request.Take ?? 50, cancellationToken);

        IEnumerable<Response> response = [.. clientes.Select(c => new Response(
            c.Id,
            c.Cpf.Valor,
            c.Nome,
            c.Email.Endereco
        ))];

        Logger.LogInformation($"Total de clientes encontrados: {response.Count()}");

        return Result.Success(response);
    }
}
