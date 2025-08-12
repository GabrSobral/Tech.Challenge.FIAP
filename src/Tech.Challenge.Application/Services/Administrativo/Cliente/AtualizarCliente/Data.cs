using Tech.Challenge.Domain.Entities.Cliente.ValueObjects;

namespace Tech.Challenge.Application.Services.Administrativo.Cliente.AtualizarCliente;

public record Request(
    Guid Id,
    CPF Cpf,
    string Nome,
    Email Email
);

public record RequestBody(
    string Cpf,
    string Nome,
    string Email
);
