namespace Tech.Challenge.Application.Services.Administrativo.Servicos.ListarServicos;

public record Request(
    int? Page,
    int? Take
);

public record Response(
    Guid Id,
    string Nome,
    decimal PrecoServico
);
