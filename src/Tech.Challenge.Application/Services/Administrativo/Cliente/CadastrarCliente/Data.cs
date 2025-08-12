using Tech.Challenge.Domain.Entities.Cliente.ValueObjects;

namespace Tech.Challenge.Application.Services.Administrativo.Cliente.CadastrarCliente;

public record Request(
    string Nome,
    string Email,
    string Cpf);

public record Response(Guid ClienteId);
