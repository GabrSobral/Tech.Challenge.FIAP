namespace Tech.Challenge.Application.Services.AcompanhamentoOrdemDeServico.ConsultarOrdemDeServico;

public record Request(
    string Cpf, 
    Guid? ordemServicoId
);

public record Response(
    Guid Id,
    string Status,
    DateTime CriadaEm,
    decimal PrecoTotal,
    IEnumerable<ProdutoResponse> Produtos,
    IEnumerable<ServicoResponse> Servicos
);

public record ProdutoResponse(
    Guid Id,
    string Nome,
    uint Quantidade,
    string Tipo ,
    decimal PrecoProduto
    
);

public record ServicoResponse(
    Guid Id,
    string Nome,
    decimal PrecoServico);
