using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tech.Challenge.Domain.Core;
using Tech.Challenge.Domain.Entities.Cliente;
using Tech.Challenge.Domain.Entities.Cliente.ValueObjects;
using Tech.Challenge.Domain.Exceptions;
using Tech.Challenge.Domain.Interfaces;
using Tech.Challenge.Domain.Interfaces.Repositories;

namespace Tech.Challenge.Application.Services.Administrativo.Cliente.CadastrarCliente;

public class CadastrarClienteService(
    ILogger<CadastrarClienteService> Logger,
    IClienteRepository ClienteRepository,
    IUnitOfWork UnitOfWork)
{

    public async Task<Result<Response>> Execute(Request request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Iniciando o processo de cadastro do cliente.");
        
        var email = Email.Criar(request.Email);

        if (email.IsFailure)
        {
            Logger.LogWarning("Email inválido: {Email}", request.Email);
            return Result.Failure<Response>(email.Error!);
        }

        var cpf = CPF.Criar(request.Cpf);

        if (cpf.IsFailure) {
            Logger.LogWarning("CPF inválido: {Cpf}", request.Cpf);
            return Result.Failure<Response>(cpf.Error!);
        }

        var clienteExistente = await ClienteRepository.GetClienteByCpf(cpf.Value, cancellationToken);

        if (clienteExistente is not null)
        {
            Logger.LogInformation($"Cliente jã cadastrado com o CPF: {cpf.Value}");
            return Result.Failure<Response>(new ClienteAlreadyRegisteredException(cpf.Value.Valor));
        }

        var cliente = Tech.Challenge.Domain.Entities.Cliente.Cliente.Criar(cpf.Value, request.Nome, email.Value);

        await ClienteRepository.AddCliente(cliente, cancellationToken);

        await UnitOfWork.SaveChangesAsync(cancellationToken);

        Logger.LogInformation($"Cliente {request.Nome} cadastrado com sucesso.");
        
        return Result.Success(new Response(cliente.Id));
    }
}
