using Tech.Challenge.Domain.Entities.Cliente.ValueObjects;
using Tech.Challenge.Domain.Enums;

namespace Tech.Challenge.Application.Services.AcompanhamentoOrdemDeServico.AprovarOrdemDeServico;

public record Request(
    CPF ClienteCpf,
    Guid OrdemServicoId
);

public record RequestBody(
    string ClienteCpf
);
