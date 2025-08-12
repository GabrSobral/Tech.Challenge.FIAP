namespace Tech.Challenge.Application.Services.Administrativo.Cliente.IdentificarCliente;

public record Request(string Cpf);

public record Response(
    Guid Id,
    string Cpf,
    string Nome,
    string Email,
    IEnumerable<VeiculoResponse> Veiculos
);

public record VeiculoResponse(
    Guid Id,
    string Placa,
    string Modelo,
    uint Ano
);