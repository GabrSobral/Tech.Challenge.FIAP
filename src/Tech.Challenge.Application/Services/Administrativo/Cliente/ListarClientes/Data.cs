namespace Tech.Challenge.Application.Services.Administrativo.Cliente.ListarClientes;

public record Request(
    int? Page,
    int? Take
);

public record Response(
    Guid Id,
    string Cpf,
    string Nome,
    string Email
);
