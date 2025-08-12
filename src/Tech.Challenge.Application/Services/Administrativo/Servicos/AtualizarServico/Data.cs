namespace Tech.Challenge.Application.Services.Administrativo.Servicos.AtualizarServico;

public record Request(
    Guid Id,
    string Nome,
    decimal PrecoServico
);

public record RequestBody(
    string Nome,
    decimal PrecoServico
);