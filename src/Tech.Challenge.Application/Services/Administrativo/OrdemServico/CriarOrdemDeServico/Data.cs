namespace Tech.Challenge.Application.Services.Administrativo.OrdemServico.CriarOrdemDeServico;

public record Request(
    Guid ClienteId,
    Guid VeiculoId,
    IEnumerable<Guid> Servicos,
    IEnumerable<ProdutosRequest> Produtos
);

public record ProdutosRequest(
    Guid Id,
    uint Quantidade
);

public record Response(
    Guid OrdemServicoId
);