namespace Tech.Challenge.Application.Services.Administrativo.Servicos.AdicionarServico;

public record Request(
    string Nome,
    decimal PrecoServico
);

public record Response(Guid ServicoId);
