using Tech.Challenge.Domain.Enums;

namespace Tech.Challenge.Application.Services.Administrativo.OrdemServico.AtualizarStatusOrdemDeServico;

public record Request(
    Guid OrdemServicoId,
    EServiceOrderStatus Status 
);

public record RequestBody(EServiceOrderStatus Status);
