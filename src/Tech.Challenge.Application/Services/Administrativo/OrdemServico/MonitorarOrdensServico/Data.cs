namespace Tech.Challenge.Application.Services.Administrativo.OrdemServico.MonitorarOrdensServico;

public record Request(
    int? Page,
    int? Take
);

public record Response(
    decimal TempoMedioEntregaMinutos,
    string TempoMedioEntrega,
    IEnumerable<OrdemServicoResponse> OrdensServico
);


public record OrdemServicoResponse(
    Guid Id,
    string Status,
    decimal PrecoTotal,
    DateTime CriadaEm,
    DateTime? EntregueEm,
    IEnumerable<ProdutoResponse> Produtos,
    IEnumerable<ServicoResponse> Servicos
);

public record ProdutoResponse(
    Guid Id,
    string Nome,
    uint Quantidade,
    string Tipo,
    decimal PrecoProduto

);

public record ServicoResponse(
    Guid Id,
    string Nome,
    decimal PrecoServico);
