using Tech.Challenge.Domain.Entities.Cliente.ValueObjects;

namespace Tech.Challenge.Application.Services.AcompanhamentoOrdemDeServico.ReprovarOrdemDeServico;

public record Request(
    CPF ClienteCpf,
    Guid OrdemServicoId
);

public record RequestBody(
    string ClienteCpf
);
